namespace Thingiverse.Domain.Models
{
    public class ThingImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }

        public int ThingId { get; set; }
        public Thing Thing { get; set; }
    }
}
