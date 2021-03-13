using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetSpot.DATA;
using PetSpot.DATA.Entities;
using PetSpot.DATA.Exceptions;
using PetSpot.DATA.Models;
using PetSpot.LOGGING;
using System.Threading.Tasks;

namespace PetSpot.API.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        private readonly PetSpotDbContext context;

        public LocationRepository(IMapper mapper,
            ILoggerManager logger,
            PetSpotDbContext context)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.context = context;
        }

        /// <summary>
        /// Creates a new location object for a specific 
        /// animal and stores it in the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateLocation(LocationBm model)
        {
            var animalLocation = await context.Locations.FirstOrDefaultAsync(x => x.AnimalId == model.AnimalId);
            if (animalLocation == null)
            {
                var location = mapper.Map<Location>(model);
                context.Locations.Add(location);
                await context.SaveChangesAsync();
                logger.LogInfo($"Location for animal {model.AnimalId} successfully added.");
            }
            else
            {
                throw new BadRequestException($"Animal with id {model.AnimalId} already has a location.");
            }
        }

        /// <summary>
        /// Retrieves animal's location by it's id.
        /// If not found, the method will throw a 404 Not Found. 
        /// </summary>
        /// <param name="animalId"></param>
        /// <returns></returns>
        public async Task<LocationBm> GetLocation(int animalId)
        {
            var animalLocation = await context.Locations.FirstOrDefaultAsync(x => x.AnimalId == animalId);
            if (animalLocation != null)
            {
                logger.LogInfo($"Location for animal {animalId} successfully retrieved");
                return mapper.Map<LocationBm>(animalLocation);
            }

            throw new NotFoundException($"Animal with id {animalId} does not have a location.");
        }

        /// <summary>
        /// Updates animal's location. This method gets called on 
        /// animal update.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateLocation(LocationBm model)
        {
            var animalLocation = await context.Locations.FirstOrDefaultAsync(x => x.AnimalId == model.AnimalId);
            if (animalLocation != null)
            {
                mapper.Map(model, animalLocation);
                context.Update(animalLocation);
                await context.SaveChangesAsync();
            }
            else
            {
                throw new NotFoundException($"Animal location does not exist");
            }
        }
    }
}
