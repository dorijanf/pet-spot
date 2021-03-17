using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetSpot.API.Configuration;
using PetSpot.API.Repositories;
using PetSpot.DATA;
using PetSpot.DATA.Entities;
using PetSpot.DATA.Exceptions;
using PetSpot.DATA.Models;
using PetSpot.LOGGING;
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
            if(animal != null)
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
    }
}
