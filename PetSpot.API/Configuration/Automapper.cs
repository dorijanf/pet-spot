using AutoMapper;
using PetSpot.DATA.Entities;
using PetSpot.DATA.Models;

namespace PetSpot.API.Configuration
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            CreateMap<TokenResultDto, AuthorizeResponseDto>();
            CreateMap<RegisterUserBm, User>();
            CreateMap<AnimalBm, Animal>();
            CreateMap<Animal, AnimalDto>();
            CreateMap<LocationBm, Location>();
            CreateMap<Location, LocationBm>();
            CreateMap<AnimalBm, LocationBm>();
        }
    }
}
