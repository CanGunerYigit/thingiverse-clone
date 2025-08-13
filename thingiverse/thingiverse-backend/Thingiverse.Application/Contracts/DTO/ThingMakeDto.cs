using Microsoft.AspNetCore.Http;


namespace Thingiverse.Application.Contracts.DTO
{
    public class ThingMakeDto
    {
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string PreviewImage { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
