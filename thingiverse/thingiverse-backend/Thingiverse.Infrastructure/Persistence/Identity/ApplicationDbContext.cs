using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Thingiverse.Domain.Models;

namespace Thingiverse.Infrastructure.Persistence.Identity
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { } 

        public DbSet<Item> Items { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<Make> Makes { get; set; }

        public DbSet<ItemLike> ItemLikes { get; set; }
        public DbSet<Thing> Things { get; set; }
        public DbSet<ThingImage> ThingImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Make>()
                .HasOne(m => m.Item)
                .WithMany(i => i.Makes)
                .HasForeignKey(m => m.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    builder.Entity<Item>()
        //        .HasMany(i => i.Makes)
        //        .WithOne(m => m.Item)
        //        .HasForeignKey(m => m.ItemId)
        //        .OnDelete(DeleteBehavior.Cascade);
        //}
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Comment>()
        //        .HasOne(c => c.Item)
        //        .WithMany(p => p.Comments)
        //        .HasForeignKey(c => c.ItemId);

        //    List<IdentityRole> roles = new List<IdentityRole>
        //    {
        //        new IdentityRole
        //        {
        //            Name = "Admin",
        //            NormalizedName = "ADMIN"
        //        },
        //         new IdentityRole
        //        {
        //            Name = "User",
        //            NormalizedName = "USER"
        //        }
        //    };
        //    modelBuilder.Entity<IdentityRole>().HasData(roles);
        //}
    }

}
