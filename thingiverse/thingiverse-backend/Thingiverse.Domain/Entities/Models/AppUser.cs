using Microsoft.AspNetCore.Identity;

namespace Thingiverse.Domain.Models
{
    public class AppUser : IdentityUser
    {
       
       
        

        public ICollection<Item> Items { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
