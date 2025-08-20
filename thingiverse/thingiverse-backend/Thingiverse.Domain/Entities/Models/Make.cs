namespace Thingiverse.Domain.Models
{
    public class Make
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public string PreviewImage { get; set; }

        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // İlişki alanı
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public List<string> ImagePaths { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
