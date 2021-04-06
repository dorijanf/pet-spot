using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSpot.API.Services;
using PetSpot.DATA.Models;
using System.Threading.Tasks;

namespace PetSpot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        public IAnimalsService AnimalsService { get; }

        public AnimalsController(IAnimalsService AnimalsService)
        {
            this.AnimalsService = AnimalsService;
        }

        [HttpPost]
        [Authorize(Roles = "Registered user")]
        public async Task<IActionResult> CreateAnimal([FromBody] AnimalBm model)
        {
            var result = await AnimalsService.CreateAnimal(model);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Registered user")]
        public async Task<IActionResult> GetAnimals()
        {
            var result = await AnimalsService.GetAnimals();
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Registered user")]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAnimal([FromRoute] int id)
        {
            await AnimalsService.DeleteAnimal(id);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Registered user")]
        [Route("{id}")]
        public async Task<IActionResult> GetAnimal([FromRoute] int id)
        {
            var result = await AnimalsService.GetAnimal(id);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Registered user")]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAnimal([FromBody] AnimalBm model, [FromRoute] int id)
        {
            var result = await AnimalsService.UpdateAnimal(model, id);
            return Ok(result);
        }

        [HttpPatch]
        [Authorize(Roles = "Registered user")]
        public async Task<IActionResult> UpdateAnimalLocation([FromBody] LocationBm model)
        {
            await AnimalsService.UpdateAnimalLocation(model);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Registered user")]
        [Route("generate-dummy-data")]
        public async Task<IActionResult> GenerateDummyData()
        {
            await AnimalsService.GenerateDummyData();
            return Ok();
        }
    }
}
