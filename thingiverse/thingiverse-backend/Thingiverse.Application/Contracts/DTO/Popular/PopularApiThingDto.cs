using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Thingiverse.Application.Contracts.DTO.Popular
{
    public class PopularApiThingDtoAllTime
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("public_url")]
        public string PublicUrl { get; set; }
        public string Thumbnail { get; set; }

        [JsonPropertyName("preview_image")]
        public string PreviewImage { get; set; }
        public PopularApiCreatorDto Creator { get; set; }

        [JsonPropertyName("like_count")]
        public int LikeCount { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("description")]

        public string Description { get; set; }
    }

}
