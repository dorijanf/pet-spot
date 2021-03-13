using PetSpot.DATA.Entities;
using PetSpot.DATA.Models;
using System.Threading.Tasks;

namespace PetSpot.API.Repositories
{
    public interface ILocationRepository
    {
        Task CreateLocation(LocationBm model);
        Task UpdateLocation(LocationBm model);
        Task<LocationBm> GetLocation(int animalId);
    }
}
