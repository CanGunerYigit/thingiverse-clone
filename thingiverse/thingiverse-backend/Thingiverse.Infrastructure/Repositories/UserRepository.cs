using Microsoft.EntityFrameworkCore;
using Thingiverse.Infrastructure.Persistence.Identity;
using Thingiverse.Application.Interfaces;
namespace Thingiverse.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<object?> GetUserByIdAsync(string id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                     u.ProfileImageUrl
                })
                .FirstOrDefaultAsync<object>();
        }
        public async Task<List<object>> GetUserCommentsAsync(string id)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == id);
            if (!userExists)
            {
                return new List<object>();
            }

            return await _context.Comments
                .Where(c => c.AppUserId == id)
                .Select(c => new
                {
                    c.Id,
                    c.Message,
                    c.CreatedAt,
                    c.ItemId
                })
                .ToListAsync<object>();
        }

        public async Task<List<object>> SearchUsersAsync(string query, string? currentUserId)
        {
            var usersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(currentUserId))
            {
                usersQuery = usersQuery.Where(u => u.Id != currentUserId);
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                var lowerQuery = query.ToLower();

                usersQuery = usersQuery.Where(u => u.UserName.ToLower().Contains(lowerQuery));
            }

            return await usersQuery
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email
                })
                .ToListAsync<object>();
        }
    }
}
