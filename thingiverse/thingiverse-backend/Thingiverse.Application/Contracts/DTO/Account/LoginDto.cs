using System.ComponentModel.DataAnnotations;

namespace Thingiverse.Application.Contracts.DTO.Account
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
