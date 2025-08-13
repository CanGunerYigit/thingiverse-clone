using Thingiverse.Domain.Models;

namespace thingiverse_backend.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}