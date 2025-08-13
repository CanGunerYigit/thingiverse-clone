using Microsoft.AspNetCore.Mvc;
using Thingiverse.Application.Interfaces;
namespace thingiverse_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewestController : ControllerBase
    {
        private readonly INewestRepository _newRepository;

        public NewestController(INewestRepository newRepository)
        {
            _newRepository = newRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetNewestItem()
        {
            var items = await _newRepository.GetNewestItemsAsync();
            return Ok(items);
        }
    }
}
