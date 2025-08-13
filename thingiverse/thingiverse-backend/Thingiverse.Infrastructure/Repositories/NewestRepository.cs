using Microsoft.EntityFrameworkCore;
using Thingiverse.Infrastructure.Persistence.Identity;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;
namespace Thingiverse.Infrastructure.Repositories
{
    public class NewestRepository : INewestRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NewestRepository(ApplicationDbContext dbContext)
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
    }
}
