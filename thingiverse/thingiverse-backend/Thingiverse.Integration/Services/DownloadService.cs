using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Thingiverse.Application.Abstractions.Interfaces;
using Thingiverse.Application.Contracts.DTO;
using Thingiverse.Application.Options;

namespace Thingiverse.Integration.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;

        public DownloadService(HttpClient httpClient, IOptions<ThingiverseOptions> options)
        {
            _httpClient = httpClient;
            _accessToken = options.Value.Token;

        }

        public async Task<List<string>> GetThingImagesAsync(int thingId)
        {
            var doc = await GetThingJsonAsync(thingId);
            var images = new List<string>();

            if (doc.RootElement.TryGetProperty("zip_data", out var zipDataEl) &&
                zipDataEl.TryGetProperty("images", out var imagesEl))
            {
                foreach (var img in imagesEl.EnumerateArray())
                {
                    if (img.TryGetProperty("url", out var urlEl) && !string.IsNullOrEmpty(urlEl.GetString()))
                        images.Add(urlEl.GetString()!);
                }
            }

            return images;
        }

        public async Task<ZipResultDto?> CreateThingZipAsync(int thingId)
        {
            var doc = await GetThingJsonAsync(thingId);

            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                // description.txt
                if (doc.RootElement.TryGetProperty("description", out var descEl))
                {
                    var descEntry = archive.CreateEntry("description.txt");
                    using var descStream = new StreamWriter(descEntry.Open());
                    await descStream.WriteAsync(descEl.GetString() ?? "");
                }

                // zip_data varsa işle
                if (doc.RootElement.TryGetProperty("zip_data", out var zipDataEl))
                {
                    // imgs
                    if (zipDataEl.TryGetProperty("images", out var imagesEl))
                    {
                        int imgIndex = 1;
                        foreach (var img in imagesEl.EnumerateArray())
                        {
                            bool added = await AddFileFromUrlAsync(archive, img, "url", $"image_{imgIndex}");
                            if (added) imgIndex++;
                        }
                    }

                    // stl vb. dosya tipi
                    if (zipDataEl.TryGetProperty("files", out var filesEl))
                    {
                        foreach (var file in filesEl.EnumerateArray())
                        {
                            string? fileName = file.GetProperty("name").GetString();
                            await AddFileFromUrlAsync(archive, file, "url", fileName ?? "file.stl");
                        }
                    }
                }
            }

            memoryStream.Position = 0;

            if (memoryStream.Length == 0)
                return null;

            return new ZipResultDto
            {
                Content = memoryStream.ToArray(),
                FileName = $"thing_{thingId}_full.zip"
            };
        }

        // thingiverse apiden veri çekme
        private async Task<JsonDocument> GetThingJsonAsync(int thingId)
        {
            var url = $"https://api.thingiverse.com/things/{thingId}?access_token={_accessToken}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(json);
        }

        // dosya ekleme
        private async Task<bool> AddFileFromUrlAsync(ZipArchive archive, JsonElement element, string propertyName, string baseName)
        {
            if (!element.TryGetProperty(propertyName, out var urlEl))
                return false;

            var fileUrl = urlEl.GetString();
            if (string.IsNullOrEmpty(fileUrl))
                return false;

            try
            {
                var bytes = await _httpClient.GetByteArrayAsync(fileUrl);
                var ext = Path.GetExtension(fileUrl.Split('?')[0]);
                var entry = archive.CreateEntry($"{baseName}{ext}", CompressionLevel.Fastest);

                using var entryStream = entry.Open();
                await entryStream.WriteAsync(bytes, 0, bytes.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
