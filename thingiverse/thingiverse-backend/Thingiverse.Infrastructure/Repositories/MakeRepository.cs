using Microsoft.EntityFrameworkCore;
using Thingiverse.Infrastructure.Persistence.Identity;
using Thingiverse.Application.Contracts.DTO;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;
namespace Thingiverse.Infrastructure.Repositories
{
    public class MakeRepository : IMakeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _uploadFolderPath;

        public MakeRepository(ApplicationDbContext context)
        {
            _context = context;
            _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "upload");
            if (!Directory.Exists(_uploadFolderPath))
                Directory.CreateDirectory(_uploadFolderPath);
        }
        public async Task<Make> CreateMakeAsync(int itemId, ThingMakeDto dto, string userId)
        {
            var item = await _context.Items.FindAsync(itemId);
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
                Item = item,
                UserId = userId
            };

            _context.Makes.Add(make);
            await _context.SaveChangesAsync();

            return make;
        }

        public async Task<List<object>> GetItemsByMostMakesAsync()
        {
            var itemsWithMakeCount = await _context.Items
                .Select(item => new
                {
                    Item = item,
                    MakeCount = _context.Makes.Count(m => m.ItemId == item.Id)
                })
                .OrderByDescending(x => x.MakeCount)
                .ToListAsync();

            return itemsWithMakeCount.Cast<object>().ToList();
        }

        public async Task<object?> GetMakeByIdAsync(int makeId)
        {
            var make = await _context.Makes
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == makeId);

            if (make == null) return null;

            var userName = make.User?.UserName ?? "Anonymous";

            return new
            {
                make.Id,
                make.Name,
                make.Thumbnail,
                make.PreviewImage,
                make.Description,
                make.CreatedAt,
                make.UserId,
                UserName = userName,
                ImagePaths = make.ImagePaths
            };
        }

        public async Task<List<object>> GetMakesByItemIdAsync(int itemId)
        {
            var makesList = await _context.Makes
                .Where(m => m.ItemId == itemId)
                .Include(m => m.User)
                .ToListAsync();

            var makesDto = new List<object>();

            foreach (var m in makesList)
            {
                string userName = m.User?.UserName ?? "Anonymous";
                string? imageUrl = m.ImagePaths?.FirstOrDefault();

                makesDto.Add(new
                {
                    m.Id,
                    m.Name,
                    m.Thumbnail,
                    m.PreviewImage,
                    m.Description,
                    m.CreatedAt,
                    m.UserId,
                    UserName = userName,
                    ImageUrl = imageUrl
                });
            }

            return makesDto;
        }
    }
}
