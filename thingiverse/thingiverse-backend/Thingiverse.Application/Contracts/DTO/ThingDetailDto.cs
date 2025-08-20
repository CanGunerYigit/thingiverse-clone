using System.ComponentModel.DataAnnotations;

namespace Thingiverse.Application.Contracts.DTO
{
    public class ThingDetailDto
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is Required")]
        [StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
        public string Description { get; set; }
    }
}
