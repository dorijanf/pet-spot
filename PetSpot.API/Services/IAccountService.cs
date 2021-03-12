using PetSpot.DATA.Models;
using System.Threading.Tasks;

namespace PetSpot.API.Services
{
    public interface IAccountService
    {
        Task<AuthorizeResponseDto> Authorize(AuthorizeBm model);
        Task Register(RegisterUserBm model);
    }
}
