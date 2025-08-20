using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Thingiverse.Application.Interfaces;
using Thingiverse.Application.Contracts.DTO.Comment;
using Thingiverse.Domain.Models;

namespace Thingiverse.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbConnection _connection;
        private readonly UserManager<AppUser> _userManager;

        public CommentRepository(IDbConnection connection, UserManager<AppUser> userManager)
        {
            _connection = connection;
            _userManager = userManager;
        }

        public async Task<Comment?> AddCommentAsync(CommentDto commentDto, string userId)
        {
            // item kontrol
            var item = await _connection.QueryFirstOrDefaultAsync<Item>(
                "SELECT * FROM Items WHERE Id = @Id",
                new { Id = commentDto.ItemId });

            if (item == null)
                return null;

            // user kontrol efden geliyor
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            var comment = new Comment
            {
                ItemId = commentDto.ItemId,
                Message = commentDto.Message,
                CreatedAt = DateTime.UtcNow,
                UserName = user.UserName,
                AppUserId = user.Id
            };

            var sql = @"INSERT INTO Comments (ItemId, Message, CreatedAt, UserName, AppUserId)
                        VALUES (@ItemId, @Message, @CreatedAt, @UserName, @AppUserId);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            var newId = await _connection.ExecuteScalarAsync<int>(sql, comment);
            comment.Id = newId;

            return comment;
        }

        public async Task<List<object>> GetCommentsByItemAsync(int itemId)
        {
            var sql = @"SELECT c.Id, c.Message, c.CreatedAt, u.Id as UserId, u.UserName
                        FROM Comments c
                        INNER JOIN AspNetUsers u ON c.AppUserId = u.Id
                        WHERE c.ItemId = @ItemId
                        ORDER BY c.CreatedAt DESC;";

            var comments = await _connection.QueryAsync(sql, new { ItemId = itemId });

            return comments.Cast<object>().ToList();
        }

        public async Task<List<object>> GetCommentsByUserAsync(string userId)
        {
            var sql = @"SELECT c.Id, c.Message, c.CreatedAt,
                               i.Id as ItemId, i.Name, i.PublicUrl, i.Thumbnail
                        FROM Comments c
                        INNER JOIN Items i ON c.ItemId = i.Id
                        WHERE c.AppUserId = @UserId
                        ORDER BY c.CreatedAt DESC;";

            var comments = await _connection.QueryAsync(sql, new { UserId = userId });

            return comments.Cast<object>().ToList();
        }
    }
}
