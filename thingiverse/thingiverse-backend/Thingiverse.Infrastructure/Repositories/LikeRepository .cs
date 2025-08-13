using Microsoft.EntityFrameworkCore;
using Thingiverse.Infrastructure.Persistence.Identity;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;

namespace Thingiverse.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly ApplicationDbContext _context;

        public LikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddLikeAsync(ItemLike like) =>
             await _context.ItemLikes.AddAsync(like);

        public async Task<Item?> GetItemByIdAsync(int itemId) => 
            await _context.Items.FindAsync(itemId);

        public async Task<List<ItemLike>> GetLikesByUserAsync(string userId) =>
             await _context.ItemLikes
                .Where(l => l.AppUserId == userId)
                .Include(l => l.Item)
                .ToListAsync();


        public async Task<ItemLike?> GetUserLikeAsync(int itemId, string userId) =>
             await _context.ItemLikes.FirstOrDefaultAsync(l => l.ItemId == itemId && l.AppUserId == userId);

        public async Task<List<int>> GetUserLikedItemIdsAsync(string userId) =>
            await _context.ItemLikes
                .Where(l => l.AppUserId == userId)
                .Select(l => l.ItemId)
                .ToListAsync();

        public void RemoveLike(ItemLike like) => //remove metotları async olamaz
            _context.ItemLikes.Remove(like);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
