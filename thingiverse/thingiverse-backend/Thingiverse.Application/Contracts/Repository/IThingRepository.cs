using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingiverse.Domain.Models;

namespace Thingiverse.Application.Contracts.Repository
{
    public interface IThingRepository
    {
        Task<Item> GetByIdAsync(int id);
        Task<List<Item>> GetAllByFilterAsync(string filter);
        Task AddAsync(Item item);
        Task SaveChangesAsync();
    }
}
