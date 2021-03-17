using Microsoft.AspNetCore.Mvc;
using PetSpot.API.Services;
using PetSpot.DATA.Models;
using System.Threading.Tasks;

namespace PetSpot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IAccountService AccountService { get; }

        public AccountController(IAccountService AccountService)
        {
            this.AccountService = AccountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authorize(AuthorizeBm model)
        {
            var result = await AccountService.Authorize(model);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserBm model)
        {
            await AccountService.Register(model);
            return Ok();
        }
    }
}
