using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingiverse.Application.Contracts.DTO;

namespace Thingiverse.Application.Abstractions.Interfaces
{
    public interface IDownloadService
    {
        Task<List<string>> GetThingImagesAsync(int thingId);
        Task<ZipResultDto?> CreateThingZipAsync(int thingId);


    }
}
