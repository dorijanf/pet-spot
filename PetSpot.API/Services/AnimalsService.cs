using AutoMapper;
using Bogus;
using Microsoft.EntityFrameworkCore;
using PetSpot.API.Configuration;
using PetSpot.API.Repositories;
using PetSpot.DATA;
using PetSpot.DATA.Entities;
using PetSpot.DATA.Exceptions;
using PetSpot.DATA.Models;
using PetSpot.LOGGING;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PetSpot.API.Services
{
    /// <inheritdoc/>
    public class AnimalsService : IAnimalsService
    {
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        private readonly PetSpotDbContext context;
        private readonly UserResolverService userResolverService;
        private readonly ILocationRepository locationRepository;

        public AnimalsService(IMapper mapper,
            ILoggerManager logger,
            PetSpotDbContext context,
            UserResolverService userResolverService,
            ILocationRepository locationRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.context = context;
            this.userResolverService = userResolverService;
            this.locationRepository = locationRepository;
        }

        /// <summary>
        /// Creates a new animal and stores it in the database. In addition
        /// it finds current user and stores the current user as the animal's creator.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>animal's id</returns>
        public async Task<int> CreateAnimal(AnimalBm model)
        {
            var animal = mapper.Map<Animal>(model);
            var userId = GetCurrentUser();

            animal.UserId = userId;

            // will be better to put in constructor of DTO or Bm
            animal.CreatedAt = DateTime.Now;
            animal.UpdatedAt = DateTime.Now;

            var userAnimals = await context.Animals.Where(x => x.UserId == userId)
                                     .FirstOrDefaultAsync(x => x.Name == model.Name);
            if (userAnimals == null)
            {
                context.Add(animal);
                await context.SaveChangesAsync();
                var animalLocation = mapper.Map<LocationBm>(model);
                animalLocation.AnimalId = animal.Id;
                await locationRepository.CreateLocation(animalLocation);
                logger.LogInfo($"Animal with id {animal.Id} successfully created.");
                return animal.Id;
            }
            else
            {
                throw new BadRequestException($"You already registered an animal with the same name.");
            }
        }

        /// <summary>
        /// Returns a list of all animals for the currently logged in user
        /// </summary>
        /// <returns>List containing animal objects</returns>
        public async Task<List<AnimalDto>> GetAnimals()
        {
            var userId = GetCurrentUser();
            var userAnimals = await context.Animals.Where(x => x.UserId == userId)
                .ToListAsync();
            var animals = new List<AnimalDto>();
            foreach (var animal in userAnimals)
            {
                var animalToBeAdded = mapper.Map<AnimalDto>(animal);
                animalToBeAdded.Location = await locationRepository.GetLocation(animal.Id);
                animals.Add(animalToBeAdded);

            }
            logger.LogInfo($"Returning {animals.Count} animals for user {userId}");
            return animals;
        }

        /// <summary>
        /// Finds an animal by its id and deletes it. If the animal can 
        /// not be found by it's id the method throws a 404 Not Found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAnimal(int id)
        {
            var animal = await GetAnimalData(id);

            if (animal != null)
            {
                context.Remove(animal);
                await context.SaveChangesAsync();
                logger.LogInfo($"Animal with Id {id} successfully deleted.");
            }
            else
            {
                throw new NotFoundException($"Animal with id {id} does not exist.");
            }
        }

        /// <summary>
        /// Finds an animal by its id and returns it's data. If the animal can 
        /// not be found by it's id the method throws a 404 Not Found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Animal Data Transfer Object</returns>
        public async Task<AnimalDto> GetAnimal(int id)
        {
            var animal = await GetAnimalData(id);

            if (animal != null)
            {
                var animalData = mapper.Map<AnimalDto>(animal);
                animalData.Location = await locationRepository.GetLocation(id);
                return animalData;
            }

            throw new NotFoundException($"Animal with id {id} does not exist.");
        }

        /// <summary>
        /// Finds an animal by its id and then updates it with the model
        /// parameter. If the animal can not be found the method throws
        /// a 404 Not Found.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> UpdateAnimal(AnimalBm model, int id)
        {
            var animal = await GetAnimalData(id);

            // will be better to put in constructor of DTO or Bm
            animal.UpdatedAt = DateTime.Now;

            if (animal != null)
            {
                animal = mapper.Map(model, animal);
                context.Update(animal);
                await context.SaveChangesAsync();
                var animalLocation = mapper.Map<LocationBm>(model);
                animalLocation.AnimalId = animal.Id;
                await locationRepository.UpdateLocation(animalLocation);
                logger.LogInfo($"Animal with id {id} successfully updated.");
                return id;
            }
            else
            {
                throw new NotFoundException($"Animal with id {id} does not exist.");
            }
        }

        /// <summary>
        /// Find an animal by its id and update its location
        /// </summary>
        /// <param name="location">object containing animalId and coordinates</param>
        /// <returns></returns>
        public async Task UpdateAnimalLocation(LocationBm location)
        {
            var animal = await GetAnimalData(location.AnimalId);
            if (animal != null)
            {
                animal.Location = mapper.Map<Location>(location);
                context.Update(animal);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException($"Animal with id {location.AnimalId} does not exist.");
            }
        }

        // A private method that retrieves the currently logged in
        private string GetCurrentUser()
        {
            var currentUser = userResolverService.GetUser();
            var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userId;
        }

        // A private method that retrieves animal data for a user.
        // If the animal's user is not the user requesting the data,
        // the method will return a 403 Forbidden error.
        private async Task<Animal> GetAnimalData(int id)
        {
            var animal = await context.Animals.FindAsync(id);
            var userId = GetCurrentUser();
            if (animal.UserId != userId)
            {
                throw new NotAuthorizedException("You are not authorized to update this animal.");
            }

            return animal;
        }


        /// <summary>
        /// Generates dummy animals and stores them in the database
        /// </summary>
        /// <returns></returns>
        public async Task GenerateDummyData()
        {
            var generatedAnimals = await GenerateDummyAnimals();
            await GenerateDummyLocations(generatedAnimals);
        }

        /// <summary>
        /// Generates Dummy animals and stores them in the databse
        /// </summary>
        /// <returns>list of all generated animals</returns>
        private async Task<List<Animal>> GenerateDummyAnimals()
        {
            var numberOfRecords = 30;

            var userId = GetCurrentUser();

            var breeds = new[] { "Chihuahua", "English Bulldog", "French Bulldog", "Dogo Argentinto",
                "Fox Terrier", "Mutt", "Poodle", "Boxer", "Miniature Schnautzer", "Medium Schnautzer",
                "Large Schnautzer", "Shih Tzu", "Shiba Inu", "Akita Inu", "Jack Russell Terrier",
                "West Highland Terrier", "Scottish Terrier", "Irish Setter", "Vizsla", "Australian Shepherd",
                "Pirenese Mountain Dog", "Dobermann", "Rottweiler", "Syberian Husky", "Alaskan Malamut",
                "Malteser", "Dalmatian", "German Shepherd", "Swiss Shepherd", "Cocker Spaniel"};

            var generatedAnimals = new List<Animal>();

            for (int i = 0; i < numberOfRecords; i++)
            {
                var animals = new Faker<Animal>()
                    .RuleFor(x => x.Name, (f, x) => f.Name.FirstName())
                    .RuleFor(x => x.Breed, (f, x) => f.PickRandom(breeds))
                    .RuleFor(x => x.Age, (f, x) => Convert.ToByte(f.Random.Number(0, 20)))
                    .RuleFor(x => x.Description, f => f.Lorem.Sentence(10))
                    .RuleFor(x => x.UserId, f => userId)
                    .RuleFor(x => x.IsDeleted, f => false)
                    .RuleFor(x => x.CreatedAt, f => DateTime.Now.AddMinutes(-30))
                    .RuleFor(x => x.UpdatedAt, f => DateTime.Now.AddMinutes(-30))
                    .RuleFor(x => x.SpeciesId, f => (int)SpeciesEnum.Dog);

                var generatedAnimal = animals.Generate();

                context.Add(generatedAnimal);
                generatedAnimals.Add(generatedAnimal);
                await context.SaveChangesAsync();
            }

            return generatedAnimals;
        }

        /// <summary>
        /// Generates Dummy locations based on the number of generated animals
        /// </summary>
        /// <param name="animals">Takes a list of animals</param>
        /// <returns></returns>
        private async Task GenerateDummyLocations(List<Animal> animals)
        {
            var animalIds = new List<int>();
            foreach (var animal in animals)
            {
                animalIds.Add(animal.Id);
            }

            foreach (var id in animalIds)
            {
                var locations = new Faker<Location>()
                    .RuleFor(x => x.AnimalId, f => id)
                    .RuleFor(x => x.CoordX, f => f.Random.Double(-90, 90))
                    .RuleFor(x => x.UpdatedAt, f => DateTime.Now.AddMinutes(-30))
                    .RuleFor(x => x.CreatedAt, f => DateTime.Now.AddMinutes(-30))
                    .RuleFor(x => x.CoordY, f => f.Random.Double(-180, 180));
                var location = locations.Generate();
                context.Add(location);
                await context.SaveChangesAsync();
            }
        }
    }
}

