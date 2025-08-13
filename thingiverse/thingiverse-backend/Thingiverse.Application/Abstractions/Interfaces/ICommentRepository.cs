using Thingiverse.Application.Contracts.DTO.Comment;
using Thingiverse.Domain.Models;

namespace Thingiverse.Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment?> AddCommentAsync(CommentDto commentDto, string userId);
        Task<List<object>> GetCommentsByUserAsync(string userId);
        Task<List<object>> GetCommentsByItemAsync(int itemId);
    }
}
