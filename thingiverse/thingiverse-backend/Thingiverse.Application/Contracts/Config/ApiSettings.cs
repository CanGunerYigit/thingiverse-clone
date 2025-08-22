using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingiverse.Application.Contracts.Config
{
    public class ApiSettings
    {
        public string ThingiverseBaseUrl { get; set; } = string.Empty;
        public string DownloadBaseUrl { get; set; } = string.Empty;
    }
}
