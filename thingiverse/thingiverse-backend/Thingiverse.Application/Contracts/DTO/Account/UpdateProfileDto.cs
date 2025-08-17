using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingiverse.Application.Contracts.DTO.Account
{
    public class UpdateProfileDto
    {
        public string? UserName { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
    