using Thingiverse.Domain.Models;

namespace Thingiverse.Application.Interfaces
{
    public interface INewestRepository
    {
        Task<List<Item>> GetNewestItemsAsync();
    }
}
