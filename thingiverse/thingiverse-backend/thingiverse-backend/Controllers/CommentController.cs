using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Thingiverse.Application.Interfaces;
using Thingiverse.Application.Contracts.DTO.Comment;

namespace thingiverse_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [Authorize]
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] CommentDto commentDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not logged in");

            var comment = await _commentRepository.AddCommentAsync(commentDto, userId);
            if (comment == null)
                return NotFound("Item or user not found");

            return Ok(comment);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCommentsByUser(string userId)
        {
            var comments = await _commentRepository.GetCommentsByUserAsync(userId);

            if (comments == null || comments.Count == 0)
                return NotFound("Kullanıcının hiç yorumu yok.");

            return Ok(comments);
        }

        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetCommentsByItem(int itemId)
        {
            var comments = await _commentRepository.GetCommentsByItemAsync(itemId);
            return Ok(comments);
        }
    }
}
