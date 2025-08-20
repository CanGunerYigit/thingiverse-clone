using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingiverse.Application.Contracts.DTO
{
    public class ItemImageDto
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Url is required.")]
        [Url(ErrorMessage = "Invalid Url format.")]
        public string Url { get; set; } = "";
    }
}
