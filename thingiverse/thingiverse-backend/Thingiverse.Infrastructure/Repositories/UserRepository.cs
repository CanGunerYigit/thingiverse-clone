using Dapper;
using System.Data;
using Thingiverse.Application.Interfaces;

namespace Thingiverse.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<object?> GetUserByIdAsync(string id)
        {
            var sql = @"
                SELECT Id, UserName, Email, ProfileImageUrl
                FROM AspNetUsers
                WHERE Id = @Id";

            return await _connection.QueryFirstOrDefaultAsync(sql, new { Id = id });
        }

        public async Task<List<object>> GetUserCommentsAsync(string id)
        {
            // kullanıcı kontrolü
            var userExists = await _connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM AspNetUsers WHERE Id = @Id",
                new { Id = id });

            if (userExists == 0)
                return new List<object>();

            var sql = @"
                SELECT Id, Message, CreatedAt, ItemId
                FROM Comments
                WHERE AppUserId = @Id";

            var comments = await _connection.QueryAsync(sql, new { Id = id });
            return comments.ToList<object>();
        }

        public async Task<List<object>> SearchUsersAsync(string query, string? currentUserId)
        {
            var sql = @"
                SELECT Id, UserName, Email
                FROM AspNetUsers
                WHERE (@CurrentUserId IS NULL OR Id != @CurrentUserId)
                  AND (@Query IS NULL OR LOWER(UserName) LIKE '%' + LOWER(@Query) + '%')";

            var users = await _connection.QueryAsync(sql, new { CurrentUserId = currentUserId, Query = query });
            return users.ToList<object>();
        }
    }
}
