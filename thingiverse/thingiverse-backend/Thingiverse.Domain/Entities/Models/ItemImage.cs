namespace Thingiverse.Domain.Models
{
    public class ItemImage
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int ThingiverseImageId { get; set; }

        public string ImageUrl { get; set; }
        public byte[] ImageData { get; set; }

        // İsteğe bağlı: dosya tipi (image/jpeg, image/png vb)
        public string ContentType { get; set; }
    }
}
