using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSpot.API.Services;
using System;
using System.Threading.Tasks;

namespace PetSpot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Registered user")]
    public class SyncController : ControllerBase
    {
        public ISyncService SyncService { get; }

        public SyncController(ISyncService syncService)
        {
            SyncService = syncService;
        }

        [HttpGet]
        [Route("get-latest")]
        public async Task<IActionResult> GetLatest(DateTime lastSync)
        {
            var result = await SyncService.GetLatest(lastSync);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await SyncService.GetAll();
            return Ok(result);
        }
    }
}
