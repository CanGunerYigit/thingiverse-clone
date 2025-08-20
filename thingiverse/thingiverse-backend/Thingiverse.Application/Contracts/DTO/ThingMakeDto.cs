using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Thingiverse.Application.Contracts.DTO
{
    public class ThingMakeDto
    {
        [Required(ErrorMessage = "Name is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Thumbnail is Required")]
        public string Thumbnail { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Preview Image is Required")]
        public string PreviewImage { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [MinLength(1, ErrorMessage = "There must be at least 1 image")]
        public List<IFormFile> Images { get; set; }
    }
}
