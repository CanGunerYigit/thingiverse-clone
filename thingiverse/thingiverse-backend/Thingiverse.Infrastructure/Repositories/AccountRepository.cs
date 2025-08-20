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

        public async Task<(bool Success, string Message)> ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "Kullanıcı bulunamadı");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors);
            }

            return (true, "Şifre başarıyla güncellendi");
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
                id = user.Id,

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

        public async Task<(bool success, string message, object user)> UpdateProfileAsync(string userId, UpdateProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "Kullanıcı bulunamadı", null);

            if (!string.IsNullOrEmpty(dto.UserName))
            {
                user.UserName = dto.UserName;
            }

            if (dto.ProfileImage != null)
            {
                // wwwroot/images içine kaydet
                var fileName = $"{Guid.NewGuid()}_{dto.ProfileImage.FileName}";
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ProfileImage.CopyToAsync(stream);
                }

                user.ProfileImageUrl = $"/images/{fileName}";
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) //hata aldıysa
                return (false, "Profil güncellenemedi", null);

            return (true, "Profil güncellendi", new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.ProfileImageUrl
            });
        }

    }
}
