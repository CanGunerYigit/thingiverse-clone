using Thingiverse.Application.Contracts.DTO;
using Thingiverse.Domain.Models;

namespace Thingiverse.Application.Interfaces
{
    public interface IMakeRepository
    {
        Task<Make> CreateMakeAsync(int itemId, ThingMakeDto dto, string userId);
        Task<List<object>> GetMakesByItemIdAsync(int itemId);
        Task<List<object>> GetItemsByMostMakesAsync();
        Task<object?> GetMakeByIdAsync(int makeId);
    }
}
