using Microsoft.AspNetCore.Http;

namespace Thingiverse.Application.Contracts.DTO
{
    public class CreateItemDto
    {
        public string Name { get; set; }
        public IFormFile? ThumbnailFile { get; set; }
        public string Description { get; set; }
        public string PreviewImage { get; set; }
        public IFormFile? PreviewImageFile { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
