using System.Data;
using Dapper;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Thingiverse.Application.Contracts.DTO;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Thingiverse.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IDbConnection _connection;
        private readonly HttpClient _httpClient;
        private readonly string _apiToken = "bb5c9468817bf1fb6718ac8ccf64a86f"; // App Token
        private readonly string _uploadFolderPath;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ItemRepository(IDbConnection connection, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _connection = connection;
            _httpClient = httpClientFactory.CreateClient();
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "upload");
            _httpContextAccessor = httpContextAccessor;


            if (!Directory.Exists(_uploadFolderPath))
                Directory.CreateDirectory(_uploadFolderPath);
        }

        public async Task<Item> CreateItemAsync(CreateItemDto dto, string userName)
        {
            int maxId = await _connection.ExecuteScalarAsync<int?>("SELECT MAX(Id) FROM Items") ?? 0;

            // Base URL http://localhost:7267
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            // thumbnail
            string? thumbnailUrl = null;
            if (dto.ThumbnailFile != null && dto.ThumbnailFile.Length > 0)
            {
                var thumbFileName = Guid.NewGuid() + Path.GetExtension(dto.ThumbnailFile.FileName);
                var thumbPath = Path.Combine(_uploadFolderPath, thumbFileName);

                using (var stream = new FileStream(thumbPath, FileMode.Create))
                {
                    await dto.ThumbnailFile.CopyToAsync(stream);
                }

                thumbnailUrl = $"{baseUrl}/upload/{thumbFileName}";
            }

            // previewimage
            string? previewImageUrl = null;
            if (dto.PreviewImageFile != null && dto.PreviewImageFile.Length > 0)
            {
                var prevFileName = Guid.NewGuid() + Path.GetExtension(dto.PreviewImageFile.FileName);
                var prevPath = Path.Combine(_uploadFolderPath, prevFileName);

                using (var stream = new FileStream(prevPath, FileMode.Create))
                {
                    await dto.PreviewImageFile.CopyToAsync(stream);
                }

                previewImageUrl = $"{baseUrl}/upload/{prevFileName}";
            }

            // item oluştur 
            var newItem = new Item
            {
                Id = maxId + 1,
                Name = dto.Name,
                Description = dto.Description ?? "",
                Thumbnail = thumbnailUrl ?? "",
                PreviewImage = previewImageUrl ?? "",
                CreatorName = userName,
                Likes = 0,
                CreatedAt = DateTime.Now,
                Images = new List<ItemImage>()
            };

            // itemi insertleme
            var insertItemSql = @"
        INSERT INTO Items (Id, Name, Description, Thumbnail, PreviewImage, CreatorName, Likes, CreatedAt)
        VALUES (@Id, @Name, @Description, @Thumbnail, @PreviewImage, @CreatorName, @Likes, @CreatedAt);";

            await _connection.ExecuteAsync(insertItemSql, newItem);

            // itemimages insertleme
            if (dto.Images != null && dto.Images.Count > 0)
            {
                foreach (var image in dto.Images)
                {
                    if (image != null && image.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine(_uploadFolderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        // otomatik thingiverseimageid atama
                        var imgParams = new
                        {
                            ItemId = newItem.Id,
                            ImageUrl = $"{baseUrl}/upload/{fileName}"
                        };

                        string insertImageSql = @"
                    INSERT INTO ItemImages (ItemId, ImageUrl)
                    OUTPUT inserted.*
                    VALUES (@ItemId, @ImageUrl);";

                        var inserted = await _connection.QuerySingleAsync<ItemImage>(insertImageSql, imgParams);

                        newItem.Images.Add(inserted);
                    }
                }
            }

            return newItem;
        }




        public async Task<Item?> GetItemByIdAsync(int id)
        {
            var sql = "SELECT * FROM Items WHERE Id = @Id";
            var item = await _connection.QueryFirstOrDefaultAsync<Item>(sql, new { Id = id });
            if (item == null) return null;

            // comment join
            var comments = await _connection.QueryAsync<Comment>(
                "SELECT * FROM Comments WHERE ItemId = @ItemId ORDER BY CreatedAt DESC",
                new { ItemId = id });

            item.Comments = comments.ToList();

            return item;
        }

        public Task<ItemImage?> GetItemImageAsync(int itemId, int imageId)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]?> GetItemImageDataAsync(int imageId)
        {
            var image = await _connection.QueryFirstOrDefaultAsync<ItemImage>(
                "SELECT * FROM ItemImages WHERE Id = @Id", new { Id = imageId });

            return image?.ImageData;
        }

        public async Task<object?> GetItemWithImagesAsync(int id)
        {
            var item = await _connection.QueryFirstOrDefaultAsync<Item>(
       "SELECT * FROM Items WHERE Id = @Id", new { Id = id });

            if (item == null) return null;

            var images = await _connection.QueryAsync<ItemImage>(
                "SELECT * FROM ItemImages WHERE ItemId = @ItemId",
                new { ItemId = id }
            );

            // thingiverse url al 
            var imageDtos = new List<object>();

            foreach (var img in images)
            {
                if (!string.IsNullOrEmpty(img.ImageUrl) && img.ImageUrl.StartsWith("http"))
                {
                    // url cdnse geri dön
                    imageDtos.Add(new
                    {
                        id = img.ThingiverseImageId,
                        itemId = item.Id,
                        url = img.ImageUrl
                    });
                }
                else if (img.ThingiverseImageId != 0) // thingiverseten çekilmişse imageid si 0 olmaz
                {
                    // metadata çek
                    var metaRequest = new HttpRequestMessage(
                        HttpMethod.Get,
                        $"https://api.thingiverse.com/things/{item.Id}/images/{img.ThingiverseImageId}"
                    );
                    metaRequest.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", _apiToken);

                    var metaResponse = await _httpClient.SendAsync(metaRequest);
                    if (metaResponse.IsSuccessStatusCode)
                    {
                        var metaJson = await metaResponse.Content.ReadAsStringAsync();
                        dynamic meta = JsonConvert.DeserializeObject(metaJson);

                        // display_large varsa kullan
                        string imageUrl = meta.sizes[0].url;
                        foreach (var size in meta.sizes)
                        {
                            if (size.type == "display_large")
                            {
                                imageUrl = size.url;
                                break;
                            }
                        }

                        imageDtos.Add(new
                        {
                            id = img.ThingiverseImageId,
                            itemId = item.Id,
                            url = imageUrl
                        });
                    }
                }
            }

            return new
            {
                id = item.Id,
                name = item.Name,
                description = item.Description,
                thumbnail = item.Thumbnail,
                creatorName = item.CreatorName,
                creatorUrl = item.CreatorUrl,
                createdAt = item.CreatedAt,
                images = imageDtos
            };
        }

        public async Task<(byte[]? Stream, string? ContentType, int StatusCode, string? ErrorMessage)> GetThingiverseImageAsync(int thingId, int imageId)
        {
            var metaRequest = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.thingiverse.com/things/{thingId}/images/{imageId}"
            );
            metaRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiToken);

            var metaResponse = await _httpClient.SendAsync(metaRequest);
            if (!metaResponse.IsSuccessStatusCode)
            {
                return (null, null, (int)metaResponse.StatusCode, "Thingiverse API metadata alınamadı.");
            }

            var metaJson = await metaResponse.Content.ReadAsStringAsync();
            dynamic meta = JsonConvert.DeserializeObject(metaJson);

            string imageUrl = meta.sizes[0].url;
            foreach (var size in meta.sizes)
            {
                if (size.type == "display")
                {
                    imageUrl = size.url;
                    break;
                }
            }

            var imageResponse = await _httpClient.GetAsync(imageUrl);
            if (!imageResponse.IsSuccessStatusCode)
            {
                return (null, null, (int)imageResponse.StatusCode, "Görsel indirilemedi.");
            }

            var contentType = imageResponse.Content.Headers.ContentType?.ToString() ?? "image/jpeg";
            var stream = await imageResponse.Content.ReadAsByteArrayAsync();

            return (stream, contentType, 200, null);
        }

        public async Task<List<Item>> SearchItemsByNameAsync(string query)
        {
            var sql = @"SELECT TOP 10 * FROM Items 
                        WHERE Name LIKE '%' + @Query + '%' 
                        ORDER BY Name ASC";

            var items = await _connection.QueryAsync<Item>(sql, new { Query = query });

            return items.ToList();
        }
    }
}
