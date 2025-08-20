using System.ComponentModel.DataAnnotations;

namespace Thingiverse.Application.Contracts.DTO.Account
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is Required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3-20 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Input type must be email format")]
        [StringLength(20, ErrorMessage = "Email must be maximum 20 characters")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8-20 characters")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
