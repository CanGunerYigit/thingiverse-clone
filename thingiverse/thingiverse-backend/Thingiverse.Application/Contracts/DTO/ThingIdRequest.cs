using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingiverse.Application.Contracts.DTO
{
    public class ThingIdRequest
    {
        [Required(ErrorMessage = "ThingId is required")]
        public int ThingId { get; set; }
    }
}
