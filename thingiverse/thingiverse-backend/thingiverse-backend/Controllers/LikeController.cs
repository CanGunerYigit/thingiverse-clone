using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml.Linq;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;

namespace thingiverse_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("toggle/{itemId:int}")]
        [Authorize]
        public async Task<IActionResult> ToggleLike(int itemId)
        {
            if (itemId <= 0)
                return BadRequest("Geçersiz item ID");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var (success, errorMessage, liked) = await _likeService.ToggleLikeAsync(itemId, userId);

            if (!success)
                return NotFound(errorMessage);

            return Ok(new { liked });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetLikesByUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("Geçersiz kullanıcı ID");
            var likes = await _likeService.GetLikesByUserAsync(userId);

            var result = likes.Select(l => new
            {
                l.Id,
                Item = new
                {
                    l.Item.Id,
                    l.Item.Name,
                    l.Item.Thumbnail
                }
            }).ToList();

            
            return Ok(result);
        }


        [HttpGet("userlikes")]
        [Authorize]
        public async Task<IActionResult> GetUserLikes()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) 
                return BadRequest();

            var likedItems = await _likeService.GetUserLikedItemIdsAsync(userId);

            return Ok(likedItems);
        }
    }
}
