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
            try
            {
                var items = await _newRepository.GetNewestItemsAsync();

                // liste boşmu 
                if (items == null || !items.Any())
                    return NotFound("Yeni ürün bulunamadı.");

                return Ok(items);
            }
            catch (Exception ex)
            {
                // hata logla
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}
