using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Thingiverse.Application.Interfaces;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers([FromQuery] string query)
    {
        var currentUserId = User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var users = await _userRepository.SearchUsersAsync(query, currentUserId);
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
            return NotFound(new { message = "User not found" });

        return Ok(user);
    }

    [HttpGet("{id}/comments")]
    public async Task<IActionResult> GetUserComments(string id)
    {
        var comments = await _userRepository.GetUserCommentsAsync(id);
        if (comments.Count == 0)
            return NotFound(new { message = "User not found or no comments" });

        return Ok(comments);
    }
}
