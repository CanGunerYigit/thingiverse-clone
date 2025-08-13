using Thingiverse.Domain.Models;

namespace Thingiverse.Application.Interfaces
{
    public interface IPopularRepository
    {
        Task<List<Item>> GetNewestItemsAsync();
        Task<List<Item>> GetPopularAllTimeAsync();
        Task<List<Item>> GetPopular10MonthsAsync();
        Task<List<Item>> GetPopular3YearsAsync();
    }
}
