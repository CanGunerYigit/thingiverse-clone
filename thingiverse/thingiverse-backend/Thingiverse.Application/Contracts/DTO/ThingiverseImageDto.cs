using System.ComponentModel.DataAnnotations;

namespace Thingiverse.Application.Contracts.DTO
{
    public class ThingiverseImageDto
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Image URL is required.")]

        public string Url { get; set; }
    }
}
