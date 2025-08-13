using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thingiverse.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int ItemId { get; set; }  // Yorumun ait olduğu ürünün ID'si

        public string UserName { get; set; }    // Yorum yapan kullanıcı adı
        public string Message { get; set; }     // Yorum mesajı
        public DateTime CreatedAt { get; set; } // Yorumun tarihi

        public Item Item { get; set; }  // Navigation property (ilişki)
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

    }
}
