using Dapper;
using System.Data;
using Thingiverse.Application.Contracts.Repository;
using Thingiverse.Domain.Models;

namespace Thingiverse.Infrastructure.Repositories
{
    public class ThingRepository : IThingRepository
    {
        private readonly IDbConnection _connection;

        public ThingRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<Item> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT i.*, im.Id AS ImageId, im.ImageUrl, im.ImageData, im.ContentType
                FROM Items i
                LEFT JOIN ItemImages im ON i.Id = im.ItemId
                WHERE i.Id = @Id";

            var itemDict = new Dictionary<int, Item>();

            var result = await _connection.QueryAsync<Item, ItemImage, Item>(
                sql,
                (item, image) =>
                {
                    if (!itemDict.TryGetValue(item.Id, out var currentItem))
                    {
                        currentItem = item;
                        currentItem.Images = new List<ItemImage>();
                        itemDict.Add(currentItem.Id, currentItem);
                    }

                    if (image != null && image.Id != 0)
                        currentItem.Images.Add(image);

                    return currentItem;
                },
                new { Id = id },
                splitOn: "ImageId"
            );

            return itemDict.Values.FirstOrDefault();
        }

        public async Task<List<Item>> GetAllByFilterAsync(string filter)
        {
            var sql = @"
                SELECT i.*, im.Id AS ImageId, im.ImageUrl, im.ImageData, im.ContentType
                FROM Items i
                LEFT JOIN ItemImages im ON i.Id = im.ItemId
                WHERE i.PopularityFilter = @Filter";

            var itemDict = new Dictionary<int, Item>();

            var result = await _connection.QueryAsync<Item, ItemImage, Item>(
                sql,
                (item, image) =>
                {
                    if (!itemDict.TryGetValue(item.Id, out var currentItem))
                    {
                        currentItem = item;
                        currentItem.Images = new List<ItemImage>();
                        itemDict.Add(currentItem.Id, currentItem);
                    }

                    if (image != null && image.Id != 0)
                        currentItem.Images.Add(image);

                    return currentItem;
                },
                new { Filter = filter },
                splitOn: "ImageId"
            );

            return itemDict.Values.ToList();
        }

        public async Task AddAsync(Item item)
        {
            var sql = @"
                INSERT INTO Items (Name, Description, CreatedAt, PopularityFilter, Likes)
                VALUES (@Name, @Description, @CreatedAt, @PopularityFilter, @Likes);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            var id = await _connection.QuerySingleAsync<int>(sql, item);
            item.Id = id;

            if (item.Images != null && item.Images.Any())
            {
                foreach (var img in item.Images)
                {
                    img.ItemId = id;
                    var imgSql = @"
                        INSERT INTO ItemImages (ItemId, ImageUrl, ImageData, ContentType)
                        VALUES (@ItemId, @ImageUrl, @ImageData, @ContentType)";
                    await _connection.ExecuteAsync(imgSql, img);
                }
            }
        }

        public async Task SaveChangesAsync()
        {
            
            await Task.CompletedTask;
        }
    }
}
