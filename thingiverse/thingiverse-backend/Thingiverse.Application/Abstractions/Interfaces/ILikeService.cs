using Thingiverse.Domain.Models;


namespace Thingiverse.Application.Interfaces
{
    public interface ILikeService
    {
        Task<(bool success, string? errorMessage, bool liked)> ToggleLikeAsync(int itemId, string userId);
        Task<List<int>> GetUserLikedItemIdsAsync(string userId);
        Task<List<ItemLike>> GetLikesByUserAsync(string userId);
    }
}
