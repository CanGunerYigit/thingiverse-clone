using Thingiverse.Application.Contracts;
using Thingiverse.Domain.Models;
using Thingiverse.Application.Contracts.DTO;

namespace Thingiverse.Application.Interfaces
{
    public interface IItemRepository
    {
        Task<Item?> GetItemByIdAsync(int id);
        Task<List<Item>> SearchItemsByNameAsync(string query);
        Task<object?> GetItemWithImagesAsync(int id);
        Task<byte[]?> GetItemImageDataAsync(int imageId);
        Task<Item> CreateItemAsync(CreateItemDto dto, string userName);
        Task<(byte[]? Stream, string? ContentType, int StatusCode, string? ErrorMessage)> GetThingiverseImageAsync(int thingId, int imageId);
    }
}
