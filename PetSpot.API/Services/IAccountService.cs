using PetSpot.DATA.Models;
using System.Threading.Tasks;

namespace PetSpot.API.Services
{
    /// <summary>
    /// The account class.
    /// It contains all methods concerned with authorization
    /// and user creation.
    /// </summary>
    public interface IAccountService
    {
        Task<AuthorizeResponseDto> Authorize(AuthorizeBm model);
        Task Register(RegisterUserBm model);
    }
}
