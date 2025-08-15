using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingiverse.Application.Contracts.DTO
{
    public class ZipResultDto
    {
        public byte[] Content { get; set; }
        public string FileName { get; set; }
    }
}
