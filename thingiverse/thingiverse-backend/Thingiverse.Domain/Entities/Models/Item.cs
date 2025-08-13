using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thingiverse.Domain.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? PublicUrl { get; set; }
        public string? PreviewImage { get; set; }
        public string? Thumbnail { get; set; }
        public string? CreatorName { get; set; }
        public string Description { get; set; }
        public string? CreatorUrl { get; set; }
        public string? PopularityFilter { get; set; }  
        public int Likes { get; set; }
        public ICollection<ItemLike> ItemLikes { get; set; } = new List<ItemLike>();

        public DateTime? CreatedAt { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<ItemImage> Images { get; set; }
         public ICollection<Make> Makes { get; set; }




    }
}
