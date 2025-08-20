using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingiverse.Application.Contracts.DTO.Account
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Current Password is Required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is Required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password should be minimum 8 characters and maximum 50 characters")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "New password confirming is Required")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
