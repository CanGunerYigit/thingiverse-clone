using Thingiverse.Domain.Models;

namespace Thingiverse.Application.Interfaces
{
    public interface ILikeRepository
    {
        Task<Item?> GetItemByIdAsync(int itemId);
        Task<ItemLike?> GetUserLikeAsync(int itemId, string userId);
        Task<List<int>> GetUserLikedItemIdsAsync(string userId);
        Task<List<ItemLike>> GetLikesByUserAsync(string userId);
        Task AddLikeAsync(ItemLike like);
        void RemoveLike(ItemLike like);
        Task SaveChangesAsync();
    }
}
