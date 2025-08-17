using Thingiverse.Application.Contracts.DTO.Account;

namespace Thingiverse.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<(bool Success, string Message, NewUserDto? User)> RegisterAsync(RegisterDto registerDto);
        Task<(bool Success, string Message, NewUserDto? User)> LoginAsync(LoginDto loginDto);
        Task<(bool success, string message, object user)> UpdateProfileAsync(string userId, UpdateProfileDto dto);
        Task<(bool Success, string Message)> ChangePasswordAsync(string userId, ChangePasswordDto dto);


    }
}
