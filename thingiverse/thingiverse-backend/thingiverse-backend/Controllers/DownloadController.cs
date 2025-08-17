using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Thingiverse.Application.Abstractions.Interfaces;
using Thingiverse.Application.Contracts.DTO;
using Thingiverse.Integration.Services;
namespace thingiverse_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IDownloadService _downloadService;

        public DownloadController(IDownloadService downloadService)
        {
            
            _downloadService = downloadService;
        }
        [HttpGet("{thingId}/images")]
        public async Task<IActionResult> GetThingImages(int thingId)
        {
            var images = await _downloadService.GetThingImagesAsync(thingId);

            if (images.Count == 0)
                return NotFound("No images found.");

            return Ok(images);
        }
        [HttpPost("zip")]
        public async Task<IActionResult> CreateZip([FromBody] ThingIdRequest req)
        {
            try
            {
                var zipFile = await _downloadService.CreateThingZipAsync(req.ThingId);
                if (zipFile == null)
                    return NotFound("No files found to zip.");

                return File(zipFile.Content, "application/zip", zipFile.FileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }




    }
}
