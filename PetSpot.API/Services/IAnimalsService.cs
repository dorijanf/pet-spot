using PetSpot.DATA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetSpot.API.Services
{
    public interface IAnimalsService
    {
        Task<int> CreateAnimal(AnimalBm model);
        Task<List<AnimalDto>> GetAnimals();
        Task<AnimalDto> GetAnimal(int id);
        Task<int> UpdateAnimal(AnimalBm model, int id);
        Task DeleteAnimal(int id);
    }
}
