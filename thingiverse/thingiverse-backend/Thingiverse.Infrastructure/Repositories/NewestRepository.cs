using Dapper;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;
using System.Data;

namespace Thingiverse.Infrastructure.Repositories
{
    public class NewestRepository : INewestRepository
    {
        private readonly IDbConnection _connection;

        public NewestRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<Item>> GetNewestItemsAsync()
        {
            var sql = @"
                SELECT TOP (1000) 
                    Id,
                    Name,
                    PublicUrl,
                    PreviewImage,
                    Thumbnail,
                    CreatorName,
                    CreatorUrl,
                    PopularityFilter,
                    Likes,
                    CreatedAt,
                    AppUserId,
                    Description,
                    ImageData
                FROM Items
                WHERE CreatedAt IS NOT NULL
                ORDER BY CreatedAt DESC";

            var items = await _connection.QueryAsync<Item>(sql);
            return items.ToList();
        }
    }
}
