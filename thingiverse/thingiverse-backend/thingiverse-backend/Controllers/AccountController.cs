using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Geçersiz form verileri" });

            var (success, message, user) = await _accountRepo.LoginAsync(loginDto);

            if (!success)
                return Unauthorized(new { message });

            return Ok(user);
        }
    }
}
