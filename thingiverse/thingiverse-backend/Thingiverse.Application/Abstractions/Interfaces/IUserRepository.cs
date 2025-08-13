namespace Thingiverse.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<List<object>> SearchUsersAsync(string query, string? currentUserId);
        Task<object?> GetUserByIdAsync(string id);
        Task<List<object>> GetUserCommentsAsync(string id);
    }
}
