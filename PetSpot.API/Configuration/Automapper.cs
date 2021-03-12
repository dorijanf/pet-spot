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
        }
    }
}
