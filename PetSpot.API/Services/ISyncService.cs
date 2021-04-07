using PetSpot.DATA.Entities;
using PetSpot.DATA.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetSpot.API.Services
{
    public interface ISyncService
    {
        public Task<SyncDto> GetAll();
        public Task<SyncDto> GetLatest(DateTime lastSync);
        public Task<List<Animal>> GetLatestAnimals();
        public Task<List<Species>> GetLatestSpecies();
        public Task<List<Location>> GetLatestLocations();
    }
}
