using Dapper;
using Thingiverse.Application.Contracts.DTO;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;
using System.Data;
using System.Text.Json;

namespace Thingiverse.Infrastructure.Repositories
{
    public class MakeRepository : IMakeRepository
    {
        private readonly IDbConnection _connection;
        private readonly string _uploadFolderPath;

        public MakeRepository(IDbConnection connection)
        {
            _connection = connection;
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "upload");
            if (!Directory.Exists(_uploadFolderPath))
                Directory.CreateDirectory(_uploadFolderPath);
        }

        public async Task<Make> CreateMakeAsync(int itemId, ThingMakeDto dto, string userId)
        {
            var item = await _connection.QueryFirstOrDefaultAsync<Item>(
       "SELECT * FROM Items WHERE Id = @Id",
       new { Id = itemId }
   );

            if (item == null)
                throw new Exception("Item not found");

            var imagePaths = new List<string>();
            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var image in dto.Images)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(_uploadFolderPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        imagePaths.Add("/upload/" + fileName);
                    }
                }
            }

            var make = new Make
            {
                Name = dto.Name,
                Thumbnail = dto.Thumbnail,
                Description = dto.Description,
                PreviewImage = dto.PreviewImage,
                CreatedAt = DateTime.Now,
                ImagePaths = imagePaths, 
                ItemId = itemId,
                UserId = userId
            };

            // jsona çevir dbye kaydet
            var sql = @"INSERT INTO Makes 
                (Name, Thumbnail, Description, PreviewImage, CreatedAt, ImagePaths, ItemId, UserId)
                VALUES
                (@Name, @Thumbnail, @Description, @PreviewImage, @CreatedAt, @ImagePathsJson, @ItemId, @UserId);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            // execute ederken json olarak gönder
            make.Id = await _connection.ExecuteScalarAsync<int>(sql, new
            {
                make.Name,
                make.Thumbnail,
                make.Description,
                make.PreviewImage,
                make.CreatedAt,
                ImagePathsJson = JsonSerializer.Serialize(make.ImagePaths),
                make.ItemId,
                make.UserId
            });

            return make;
        }

        public async Task<List<object>> GetItemsByMostMakesAsync()
        {
            var sql = @"
                SELECT 
                    i.Id, i.Name, i.Description, i.Thumbnail, 
                    COUNT(m.Id) AS MakeCount
                FROM Items i
                LEFT JOIN Makes m ON i.Id = m.ItemId
                GROUP BY i.Id, i.Name, i.Description, i.Thumbnail
                ORDER BY MakeCount DESC";

            var result = await _connection.QueryAsync(sql);
            return result.ToList<object>();
        }

        public async Task<object?> GetMakeByIdAsync(int makeId)
        {
            var sql = @"
                SELECT m.Id, m.Name, m.Description, m.Thumbnail, m.PreviewImage, m.CreatedAt, m.ImagePaths, m.ItemId, m.UserId, u.UserName
                FROM Makes m
                LEFT JOIN AspNetUsers u ON m.UserId = u.Id
                WHERE m.Id = @Id";

            var make = await _connection.QueryFirstOrDefaultAsync(sql, new { Id = makeId });
            return make;
        }

        public async Task<List<object>> GetMakesByItemIdAsync(int itemId)
        {
            var sql = @"
                SELECT m.Id, m.Name, m.Description, m.Thumbnail, m.PreviewImage, m.CreatedAt, m.ImagePaths, m.ItemId, m.UserId, u.UserName
                FROM Makes m
                LEFT JOIN AspNetUsers u ON m.UserId = u.Id
                WHERE m.ItemId = @ItemId";

            var makes = await _connection.QueryAsync(sql, new { ItemId = itemId });
            return makes.ToList<object>();
        }
    }
}
