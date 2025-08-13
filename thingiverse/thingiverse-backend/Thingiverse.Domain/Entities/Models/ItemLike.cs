using System.ComponentModel.DataAnnotations;

namespace Thingiverse.Domain.Models
{
    public class ItemLike
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }
        public Item Item { get; set; }

        [Required]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
