using Microsoft.AspNetCore.Mvc;
using Thingiverse.Application.Interfaces;
using Thingiverse.Infrastructure.Repositories;

namespace thingiverse_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThingiVerseController : ControllerBase
    {
        private readonly IPopularRepository _popularRepository;

        public ThingiVerseController(IPopularRepository popularRepository)
        {
            _popularRepository = popularRepository;
        }

        [HttpGet("popular/alltime")]
        public async Task<IActionResult> GetPopularAllTime()
        {
            var items = await _popularRepository.GetPopularAllTimeAsync();
            return Ok(items);
        }

        [HttpGet("popular/10months")]
        public async Task<IActionResult> GetPopular10Months()
        {
            var items = await _popularRepository.GetPopular10MonthsAsync();
            return Ok(items);
        }

        [HttpGet("popular/3years")]
        public async Task<IActionResult> GetPopular3Years()
        {
            var items = await _popularRepository.GetPopular3YearsAsync();
            return Ok(items);
        }
    }
}
