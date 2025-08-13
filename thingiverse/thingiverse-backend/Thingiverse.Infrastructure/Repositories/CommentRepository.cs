using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thingiverse.Application.Interfaces;
using Thingiverse.Infrastructure.Persistence.Identity;
using Thingiverse.Application.Contracts.DTO.Comment;
using Thingiverse.Domain.Models;


namespace Thingiverse.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CommentRepository(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<Comment?> AddCommentAsync(CommentDto commentDto, string userId)
        {
            var item = await _context.Items.FindAsync(commentDto.ItemId);
            if (item == null)
                return null;

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

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<List<object>> GetCommentsByItemAsync(int itemId)
        {
            var comments = await _context.Comments
                .Where(c => c.ItemId == itemId)
                .Include(c => c.AppUser)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new
                {
                    c.Id,
                    c.Message,
                    c.CreatedAt,
                    UserId = c.AppUser.Id,
                    UserName = c.AppUser.UserName
                })
                .ToListAsync();

            return comments.Cast<object>().ToList();
        }

        public async Task<List<object>> GetCommentsByUserAsync(string userId)
        {
            var comments = await _context.Comments
                .Where(c => c.AppUserId == userId)
                .Include(c => c.Item)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new
                {
                    c.Id,
                    c.Message,
                    c.CreatedAt,
                    Item = new
                    {
                        c.Item.Id,
                        c.Item.Name,
                        c.Item.PublicUrl,
                        c.Item.Thumbnail
                    }
                })
                .ToListAsync();

            return comments.Cast<object>().ToList();
        }
    }
}
