using Microsoft.EntityFrameworkCore;
using Thingiverse.Infrastructure.Persistence.Identity;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;
namespace Thingiverse.Infrastructure.Repositories
{
    public class PopularRepository : IPopularRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public PopularRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Item>> GetNewestItemsAsync()
        {
            return await _dbContext.Items
                .Where(p => p.CreatedAt != null)
                .Include(i => i.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Item>> GetPopular10MonthsAsync()
        {
            var tenMonthsAgo = DateTime.UtcNow.AddMonths(-10);
            return await _dbContext.Items
                .Where(p => p.CreatedAt >= tenMonthsAgo)
                .OrderByDescending(p => p.Likes)
                .ToListAsync();
        }

        public async Task<List<Item>> GetPopular3YearsAsync()
        {
            var threeYearsAgo = DateTime.UtcNow.AddYears(-3);
            return await _dbContext.Items
                .Where(p => p.CreatedAt >= threeYearsAgo)
                .OrderByDescending(p => p.Likes)
                .ToListAsync();
        }
        public async Task<List<Item>> GetPopularAllTimeAsync()
        {
            return await _dbContext.Items
                .Include(i => i.Comments)
                .OrderByDescending(p => p.Likes)
                .ToListAsync();
        }
    }
}
