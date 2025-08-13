using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;

namespace Thingiverse.Application.Services
{
    public class LikeService : ILikeService //return işlemleri arttırma azaltma vb 
    {
        private readonly ILikeRepository _likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<List<ItemLike>> GetLikesByUserAsync(string userId) =>
            await _likeRepository.GetLikesByUserAsync(userId);

        public async Task<List<int>> GetUserLikedItemIdsAsync(string userId) =>
             await _likeRepository.GetUserLikedItemIdsAsync(userId);

        public async Task<(bool success, string? errorMessage, bool liked)> ToggleLikeAsync(int itemId, string userId)
        {
            var item = await _likeRepository.GetItemByIdAsync(itemId);
            if (item == null)
                return (false, "Item bulunamadı.", false);

            var existingLike = await _likeRepository.GetUserLikeAsync(itemId, userId);

            if (existingLike == null)
            {
                await _likeRepository.AddLikeAsync(new ItemLike { ItemId = itemId, AppUserId = userId });
                item.Likes += 1;
            }
            else
            {
                _likeRepository.RemoveLike(existingLike);
                if (item.Likes > 0)
                    item.Likes -= 1;
            }

            await _likeRepository.SaveChangesAsync();
            return (true, null, existingLike == null);
        }
    }
}
