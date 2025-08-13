using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Thingiverse.Application.Contracts.DTO;
using Thingiverse.Application.Interfaces;
namespace thingiverse_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemsController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _itemRepository.GetItemByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpGet("search/{query}")]
        public async Task<IActionResult> SearchItemsByName(string query)
        {
            var items = await _itemRepository.SearchItemsByNameAsync(query);
            return Ok(items);
        }

        [HttpGet("{id}/with-images")]
        public async Task<IActionResult> GetItemWithImages(int id)
        {
            var result = await _itemRepository.GetItemWithImagesAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("image/{thingId}/{imageId}")]
        public async Task<IActionResult> GetThingiverseImage(int thingId, int imageId)
        {
            var (stream, contentType, statusCode, errorMessage) = await _itemRepository.GetThingiverseImageAsync(thingId, imageId);
            if (stream == null)
                return StatusCode(statusCode, errorMessage);

            return File(stream, contentType!);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateItem([FromForm] CreateItemDto dto)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name ?? "Anonymous";

            var newItem = await _itemRepository.CreateItemAsync(dto, userName);

            return Ok(newItem);
        }

        [HttpGet("image/{imageId}")]
        public async Task<IActionResult> GetImage(int imageId)
        {
            var imageData = await _itemRepository.GetItemImageDataAsync(imageId);
            if (imageData == null)
                return NotFound();

            return File(imageData, "image/jpeg");
        }
    }
}
