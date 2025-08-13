using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Thingiverse.Infrastructure.Persistence.Identity;
using Thingiverse.Application.Contracts.DTO;
using Thingiverse.Application.Interfaces;
using Microsoft.AspNetCore.Http;

using Thingiverse.Domain.Models;


namespace Thingiverse.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiToken = "bb5c9468817bf1fb6718ac8ccf64a86f"; // App Token
        private readonly string _uploadFolderPath;

        public ItemRepository(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "upload");

            if (!Directory.Exists(_uploadFolderPath))
                Directory.CreateDirectory(_uploadFolderPath);
        }
        public async Task<Item> CreateItemAsync(CreateItemDto dto, string userName)
        {
            int maxId = await _context.Items.MaxAsync(i => (int?)i.Id) ?? 0;

            string? thumbnailUrl = null;
            if (dto.ThumbnailFile != null && dto.ThumbnailFile.Length > 0)
            {
                var thumbFileName = Guid.NewGuid() + Path.GetExtension(dto.ThumbnailFile.FileName);
                var thumbPath = Path.Combine(_uploadFolderPath, thumbFileName);
                using (var stream = new FileStream(thumbPath, FileMode.Create))
                {
                    await dto.ThumbnailFile.CopyToAsync(stream);
                }
                thumbnailUrl = "/upload/" + thumbFileName;
            }

            string? previewImageUrl = null;
            if (dto.PreviewImageFile != null && dto.PreviewImageFile.Length > 0)
            {
                var prevFileName = Guid.NewGuid() + Path.GetExtension(dto.PreviewImageFile.FileName);
                var prevPath = Path.Combine(_uploadFolderPath, prevFileName);
                using (var stream = new FileStream(prevPath, FileMode.Create))
                {
                    await dto.PreviewImageFile.CopyToAsync(stream);
                }
                previewImageUrl = "/upload/" + prevFileName;
            }

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

                        newItem.Images.Add(new ItemImage
                        {
                            ImageUrl = "/upload/" + fileName
                        });
                    }
                }
            }

            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();

            return newItem;
        }

       

        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _context.Items
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<byte[]?> GetItemImageDataAsync(int imageId)
        {
            var image = await _context.ItemImages.FindAsync(imageId);
            return image?.ImageData;
        }
        public async Task<object?> GetItemWithImagesAsync(int id)
        {
            var item = await _context.Items
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return null;

            return new
            {
                id = item.Id,
                name = item.Name,
                description = item.Description,
                thumbnail = item.Thumbnail,
                creatorName = item.CreatorName,
                creatorUrl = item.CreatorUrl,
                createdAt = item.CreatedAt,
                images = item.Images.Select(img => new
                {
                    id = img.ThingiverseImageId,
                    thingId = item.Id,
                    url = img.ImageUrl
                }).ToList()
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
            return await _context.Items
                .Where(x => x.Name.Contains(query))
                .OrderBy(x => x.Name)
                .Take(10)
                .ToListAsync();
        }
    }
}
