using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Thingiverse.Application.Contracts.DTO
{
    public class CreateItemDto
    {
        [Required(ErrorMessage = "Item name is Required")]
        [StringLength(50, ErrorMessage = "Item name can be max 50 characters")]
        public string Name { get; set; }
        public IFormFile? ThumbnailFile { get; set; } //nullable

        [StringLength(500, ErrorMessage = "Description can be max 500 characters")]
        public string Description { get; set; }
        public string PreviewImage { get; set; }
        public IFormFile? PreviewImageFile { get; set; } //nullable
        public List<IFormFile> Images { get; set; }
    }
}
