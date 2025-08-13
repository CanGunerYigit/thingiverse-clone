using System.Text.Json.Serialization;


namespace Thingiverse.Application.Contracts.DTO.Popular
{
    public class PopularApiCreatorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("public_url")]
        public string PublicUrl { get; set; }
    }
}
