using PetSpot.DATA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetSpot.API.Services
{
    /// <summary>
    /// Animal service interface. It contains all methods
    /// concerning animal management. 
    /// </summary>
    public interface IAnimalsService
    {
        Task<int> CreateAnimal(AnimalBm model);
        Task<List<AnimalDto>> GetAnimals();
        Task<AnimalDto> GetAnimal(int id);
        Task<int> UpdateAnimal(AnimalBm model, int id);
        Task DeleteAnimal(int id);
        Task UpdateAnimalLocation(LocationBm model);
    }
}
