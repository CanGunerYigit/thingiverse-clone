using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thingiverse.Infrastructure.Persistence.Identity;
using Thingiverse.Integration.Services;

namespace thingiverse_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddingItemToDb : ControllerBase
    {

        private readonly ThingiverseService _thingiverseService;
        
        public AddingItemToDb(ThingiverseService thingiverseService)
        {
            _thingiverseService = thingiverseService;
            

        }


        [HttpGet("import")]
        public async Task<IActionResult> Import() //itemları veritabanına eklemek 
        {
            await _thingiverseService.FetchAndSaveAllPopularThingsAsync("all_time");


            return Ok("Veriler veritabanına kaydedildi.");
        }
    }
}
