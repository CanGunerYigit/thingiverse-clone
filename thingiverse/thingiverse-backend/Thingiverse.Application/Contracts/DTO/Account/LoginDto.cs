using System.ComponentModel.DataAnnotations;

namespace Thingiverse.Application.Contracts.DTO.Account
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username cannot be empty")]
        [StringLength(20, ErrorMessage = "Username must be maximum 20 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8-20 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
