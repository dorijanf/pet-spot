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
        public IAccountService accountService { get; }

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authorize(AuthorizeBm model)
        {
            var result = await accountService.Authorize(model);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserBm model)
        {
            await accountService.Register(model);
            return Ok("User seccessfully registered.");
        }
    }
}
