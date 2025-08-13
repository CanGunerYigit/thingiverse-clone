using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Thingiverse.Domain.Models;
using Thingiverse.Application.Contracts.Repository;
using Thingiverse.Infrastructure.Persistence.Identity;

namespace Thingiverse.Infrastructure.Repositories
{
    public class ThingRepository : IThingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ThingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Item> GetByIdAsync(int id)
        {
            return await _dbContext.Items
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Item>> GetAllByFilterAsync(string filter)
        {
            return await _dbContext.Items
                .Include(i => i.Images)
                .Where(i => i.PopularityFilter == filter)
                .ToListAsync();
        }

        public async Task AddAsync(Item item)
        {
            await _dbContext.Items.AddAsync(item);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
