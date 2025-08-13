using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thingiverse.Application.Contracts.DTO.Account;
using Thingiverse.Application.Interfaces;
using Thingiverse.Domain.Models;
using thingiverse_backend.Interfaces;
using thingiverse_backend.Migrations;

namespace Thingiverse.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signIn;
        private readonly ITokenService _tokenService;

        public AccountRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signIn, ITokenService tokenService)
        {
            _userManager = userManager;
            _signIn = signIn;
            _tokenService = tokenService;
        }
        public async Task<(bool Success, string Message, NewUserDto? User)> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDto.Username.ToLower());

            if (user == null)
                return (false, "Kullanıcı adı yanlış.", null);

            var result = await _signIn.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return (false, "Şifre yanlış.", null);
            var savedUser = await _userManager.FindByNameAsync(user.UserName);

            var newUser = new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(savedUser)
            };

            return (true, "Giriş başarılı", newUser);
        }

        public async Task<(bool Success, string Message, NewUserDto? User)> RegisterAsync(RegisterDto registerDto)
        {
            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!createdUser.Succeeded)
            {
                var error = createdUser.Errors.FirstOrDefault()?.Description ?? "Kayıt başarısız.";
                return (false, error, null);
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
            {
                var error = roleResult.Errors.FirstOrDefault()?.Description ?? "Rol ekleme başarısız.";
                return (false, error, null);
            }

            var newUser = new NewUserDto
            {
                UserName = appUser.UserName,
                Email = appUser.Email,
                Token = _tokenService.CreateToken(appUser)
            };

            return (true, "Kayıt başarılı", newUser);
        }
    }
}
