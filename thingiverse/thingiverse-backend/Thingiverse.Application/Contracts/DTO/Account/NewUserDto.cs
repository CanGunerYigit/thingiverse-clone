using System.ComponentModel.DataAnnotations;

namespace Thingiverse.Application.Contracts.DTO.Account
{
    public class NewUserDto
    {
        [Required]
        public string id { get; set; }
        [Required(ErrorMessage ="UserName is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
