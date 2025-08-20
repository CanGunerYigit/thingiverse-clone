using Dapper;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;
using System.Data;

namespace Thingiverse.Infrastructure.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly IDbConnection _connection;

        public LikeRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task AddLikeAsync(ItemLike like)
        {
            var sql = @"INSERT INTO ItemLikes (ItemId, AppUserId) 
                        VALUES (@ItemId, @AppUserId)";
            await _connection.ExecuteAsync(sql, like);
        }

        public async Task<Item?> GetItemByIdAsync(int itemId)
        {
            var sql = "SELECT * FROM Items WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<Item>(sql, new { Id = itemId });
        }

        public async Task<List<ItemLike>> GetLikesByUserAsync(string userId)
        {
            var sql = @"SELECT il.*, i.* FROM ItemLikes il
                        INNER JOIN Items i ON il.ItemId = i.Id
                        WHERE il.AppUserId = @UserId";

            var lookup = new Dictionary<int, ItemLike>();

            var result = await _connection.QueryAsync<ItemLike, Item, ItemLike>(
                sql,
                (like, item) =>
                {
                    if (!lookup.TryGetValue(like.ItemId, out var il))
                    {
                        il = like;
                        il.Item = item;
                        lookup.Add(like.ItemId, il);
                    }
                    return il;
                },
                new { UserId = userId },
                splitOn: "Id"
            );

            return result.Distinct().ToList();
        }

        public async Task<ItemLike?> GetUserLikeAsync(int itemId, string userId)
        {
            var sql = @"SELECT * FROM ItemLikes 
                        WHERE ItemId = @ItemId AND AppUserId = @UserId";
            return await _connection.QueryFirstOrDefaultAsync<ItemLike>(sql, new { ItemId = itemId, UserId = userId });
        }

        public async Task<List<int>> GetUserLikedItemIdsAsync(string userId)
        {
            var sql = @"SELECT ItemId FROM ItemLikes WHERE AppUserId = @UserId";
            var ids = await _connection.QueryAsync<int>(sql, new { UserId = userId });
            return ids.ToList();
        }

        public void RemoveLike(ItemLike like)
        {
           
            var sql = @"DELETE FROM ItemLikes WHERE Id = @Id";
            _connection.Execute(sql, new { like.Id });
        }


        public async Task RemoveLikeAsync(ItemLike like)
        {
            var sql = @"DELETE FROM ItemLikes WHERE Id = @Id";
            await _connection.ExecuteAsync(sql, new { like.Id });
        }

        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}
