using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Thingiverse.Application.Contracts.DTO.Popular
{
    public class PopularApiCreatorDto
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters.")]
        public string Name { get; set; }
        [JsonPropertyName("public_url")]
        public string PublicUrl { get; set; }
    }
}
