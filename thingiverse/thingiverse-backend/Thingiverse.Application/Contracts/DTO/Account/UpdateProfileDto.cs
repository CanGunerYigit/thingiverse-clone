using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingiverse.Application.Contracts.DTO.Account
{
    public class UpdateProfileDto // alanlar nullabe required olamaz
    {
        [StringLength(100, ErrorMessage = "UserName cant be longer than 100 character")]
        public string? UserName { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
    