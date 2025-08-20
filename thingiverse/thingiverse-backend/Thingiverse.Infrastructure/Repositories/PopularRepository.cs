using Dapper;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;
using System.Data;

namespace Thingiverse.Infrastructure.Repositories
{
    public class PopularRepository : IPopularRepository
    {
        private readonly IDbConnection _connection;

        public PopularRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<Item>> GetNewestItemsAsync()
        {
            var sql = @"
                SELECT i.*, 
                       c.Id AS CommentId, 
                       c.Message AS Message, 
                       c.CreatedAt AS CommentCreatedAt, 
                       c.UserName AS CommentUserName, 
                       c.AppUserId AS AppUserId, 
                       c.ItemId
                FROM Items i
                LEFT JOIN Comments c ON i.Id = c.ItemId
                WHERE i.CreatedAt IS NOT NULL
                ORDER BY i.CreatedAt DESC";

            var itemDict = new Dictionary<int, Item>();

            var result = await _connection.QueryAsync<Item, Comment, Item>(
                sql,
                (item, comment) =>
                {
                    if (!itemDict.TryGetValue(item.Id, out var currentItem))
                    {
                        currentItem = item;
                        currentItem.Comments = new List<Comment>();
                        itemDict.Add(currentItem.Id, currentItem);
                    }

                    if (comment != null && comment.Id != 0)
                        currentItem.Comments.Add(comment);

                    return currentItem;
                },
                splitOn: "CommentId"
            );

            return itemDict.Values.ToList();
        }

        public async Task<List<Item>> GetPopular10MonthsAsync()
        {
            var tenMonthsAgo = DateTime.UtcNow.AddMonths(-10);

            var sql = @"
                SELECT i.*, 
                       c.Id AS CommentId, 
                       c.Message AS Message, 
                       c.CreatedAt AS CommentCreatedAt, 
                       c.UserName AS CommentUserName, 
                       c.AppUserId AS AppUserId, 
                       c.ItemId
                FROM Items i
                LEFT JOIN Comments c ON i.Id = c.ItemId
                WHERE i.CreatedAt >= @TenMonthsAgo
                ORDER BY i.Likes DESC";

            var itemDict = new Dictionary<int, Item>();

            var result = await _connection.QueryAsync<Item, Comment, Item>(
                sql,
                (item, comment) =>
                {
                    if (!itemDict.TryGetValue(item.Id, out var currentItem))
                    {
                        currentItem = item;
                        currentItem.Comments = new List<Comment>();
                        itemDict.Add(currentItem.Id, currentItem);
                    }

                    if (comment != null && comment.Id != 0)
                        currentItem.Comments.Add(comment);

                    return currentItem;
                },
                new { TenMonthsAgo = tenMonthsAgo },
                splitOn: "CommentId"
            );

            return itemDict.Values.ToList();
        }

        public async Task<List<Item>> GetPopular3YearsAsync()
        {
            var threeYearsAgo = DateTime.UtcNow.AddYears(-3);

            var sql = @"
                SELECT i.*, 
                       c.Id AS CommentId, 
                       c.Message AS Message, 
                       c.CreatedAt AS CommentCreatedAt, 
                       c.UserName AS CommentUserName, 
                       c.AppUserId AS AppUserId, 
                       c.ItemId
                FROM Items i
                LEFT JOIN Comments c ON i.Id = c.ItemId
                WHERE i.CreatedAt >= @ThreeYearsAgo
                ORDER BY i.Likes DESC";

            var itemDict = new Dictionary<int, Item>();

            var result = await _connection.QueryAsync<Item, Comment, Item>(
                sql,
                (item, comment) =>
                {
                    if (!itemDict.TryGetValue(item.Id, out var currentItem))
                    {
                        currentItem = item;
                        currentItem.Comments = new List<Comment>();
                        itemDict.Add(currentItem.Id, currentItem);
                    }

                    if (comment != null && comment.Id != 0)
                        currentItem.Comments.Add(comment);

                    return currentItem;
                },
                new { ThreeYearsAgo = threeYearsAgo },
                splitOn: "CommentId"
            );

            return itemDict.Values.ToList();
        }

        public async Task<List<Item>> GetPopularAllTimeAsync()
        {
            var sql = @"
                SELECT i.*, 
                       c.Id AS CommentId, 
                       c.Message AS Message, 
                       c.CreatedAt AS CommentCreatedAt, 
                       c.UserName AS CommentUserName, 
                       c.AppUserId AS AppUserId, 
                       c.ItemId
                FROM Items i
                LEFT JOIN Comments c ON i.Id = c.ItemId
                ORDER BY i.Likes DESC";

            var itemDict = new Dictionary<int, Item>();

            var result = await _connection.QueryAsync<Item, Comment, Item>(
                sql,
                (item, comment) =>
                {
                    if (!itemDict.TryGetValue(item.Id, out var currentItem))
                    {
                        currentItem = item;
                        currentItem.Comments = new List<Comment>();
                        itemDict.Add(currentItem.Id, currentItem);
                    }

                    if (comment != null && comment.Id != 0)
                        currentItem.Comments.Add(comment);

                    return currentItem;
                },
                splitOn: "CommentId"
            );

            return itemDict.Values.ToList();
        }
    }
}
