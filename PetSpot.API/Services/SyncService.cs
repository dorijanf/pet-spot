using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetSpot.DATA;
using PetSpot.DATA.Entities;
using PetSpot.DATA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetSpot.API.Services
{
    public class SyncService : ISyncService
    {
        private readonly PetSpotDbContext context;

        DateTime LastSync;

        public SyncService(PetSpotDbContext context)
        {
            this.context = context;
        }

        public async Task<SyncDto> GetAll()
        {
            return new SyncDto
            {
                Animals = await context.Animals.ToListAsync(),
                Locations = await context.Locations.ToListAsync(),
                Species = await context.Species.ToListAsync()
            };
        }

        public async Task<SyncDto> GetLatest(DateTime lastSync)
        {
            LastSync = lastSync;

            return new SyncDto
            {
                Animals = await GetLatestAnimals(),
                Locations = await GetLatestLocations(),
                Species = await GetLatestSpecies()
            };
        }

        public async Task<List<Animal>> GetLatestAnimals()
        {
            return await context.Animals
                .Where(x => x.UpdatedAt > LastSync)
                .ToListAsync();
        }

        public async Task<List<Location>> GetLatestLocations()
        {
            return await context.Locations
                .Where(x => x.UpdatedAt > LastSync)
                .ToListAsync();
        }

        public async Task<List<Species>> GetLatestSpecies()
        {
            return await context.Species
                .Where(x => x.UpdatedAt > LastSync)
                .ToListAsync();
        }
    }
}
