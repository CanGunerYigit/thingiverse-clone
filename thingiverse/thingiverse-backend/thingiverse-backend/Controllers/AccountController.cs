using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using System.Threading.Tasks;
using Thingiverse.Application.Contracts.DTO.Account;
using Thingiverse.Application.Interfaces;

namespace thingiverse_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;

        public AccountController(IAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Geçersiz form verileri" });

            var (success, message, user) = await _accountRepo.RegisterAsync(registerDto);

            if (!success)
                return BadRequest(new { message });

            return Ok(user);
        }

        [HttpPost("login")]
        [EnableRateLimiting("fixed")] //brute force için rate limit
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Geçersiz form verileri" });

            var (success, message, user) = await _accountRepo.LoginAsync(loginDto);

            if (!success)
                return Unauthorized(new { message });

            return Ok(user);
        }

        [HttpPut("update-profile")]
        [Authorize]

        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // userid al

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Kullanıcı girişi gerekli" });

            var (success, message, user) = await _accountRepo.UpdateProfileAsync(userId, dto);

            if (!success)
                return BadRequest(new { message });

            return Ok(user);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Geçersiz form verileri" });

            if (dto.NewPassword != dto.ConfirmPassword)
                return BadRequest(new { message = "Yeni şifreler eşleşmiyor" });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Kullanıcı girişi gerekli" });

            var (success, message) = await _accountRepo.ChangePasswordAsync(userId, dto);

            if (!success)
                return BadRequest(new { message });

            return Ok(new { message = "Şifre başarıyla güncellendi" });
        }
    }
}
