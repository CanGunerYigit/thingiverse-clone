namespace Thingiverse.Domain.Models
{
    public class Thing
    {
        public int Id { get; set; }

        // Kullanıcıdan gelen alanlar
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string PreviewImage { get; set; }

        // DB tarafında saklanacak alanlar
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }

        // Resimleri ayrı tablodan yönetmek daha doğru ama
        // istersen Images için sadece path tutabiliriz
        public List<ThingImage> Images { get; set; }
    }
}
