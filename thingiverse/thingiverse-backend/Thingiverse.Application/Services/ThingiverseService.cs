using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Thingiverse.Domain.Models;
using System.Text.Json.Serialization;
using Thingiverse.Application.Contracts.DTO;
using Thingiverse.Application.Contracts.DTO.Popular;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Thingiverse.Application.Contracts.Repository;
namespace Thingiverse.Application.Services
{
public class ThingiverseService
{
    private readonly HttpClient _httpClient;
    private readonly IThingRepository _thingRepository;
    private readonly string _apiToken = "bb5c9468817bf1fb6718ac8ccf64a86f";

    public ThingiverseService(HttpClient httpClient, IThingRepository thingRepository)
    {
        _httpClient = httpClient;
        _thingRepository = thingRepository;
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer bb5c9468817bf1fb6718ac8ccf64a86f");
    }
    public async Task<List<ThingiverseImageDto>> FetchItemImagesAsync(int thingId)
    {
        string url = $"https://api.thingiverse.com/things/{thingId}/images";
        var images = await _httpClient.GetFromJsonAsync<List<ThingiverseImageDto>>(url);
        return images ?? new List<ThingiverseImageDto>();
    }


    public async Task FetchAndSaveAllPopularThingsAsync(string filter, int totalPages = 50, int perPage = 20)
    {
        for (int page = 1; page <= totalPages; page++)
        {
            string url = $"https://api.thingiverse.com/popular?filter={filter}&page={page}&per_page={perPage}";

            List<PopularApiThingDtoAllTime> things = null;
            int retryCount = 0;

            while (retryCount < 3)
            {
                try
                {
                    things = await _httpClient.GetFromJsonAsync<List<PopularApiThingDtoAllTime>>(url);
                    break;
                }
                catch (HttpRequestException ex) when ((int?)ex.StatusCode == 429)
                {
                    retryCount++;
                    await Task.Delay(3000);
                }
            }

            if (things == null || things.Count == 0)
                break;

            foreach (var thing in things)
            {
                var existing = await _thingRepository.GetByIdAsync(thing.Id);

                if (existing == null)
                {
                    var detail = await FetchThingDetailAsync(thing.Id);

                    var newThing = new Item
                    {
                        Id = thing.Id,
                        Name = thing.Name,
                        PublicUrl = thing.PublicUrl ?? "",
                        Thumbnail = thing.Thumbnail ?? "",
                        Description = detail?.Description ?? "",
                        PreviewImage = thing.PreviewImage ?? "",
                        CreatorName = thing.Creator?.Name ?? "",
                        CreatorUrl = thing.Creator?.PublicUrl ?? "",
                        PopularityFilter = filter,
                        Likes = thing.LikeCount,
                        CreatedAt = thing.CreatedAt,
                        Images = new List<ItemImage>()
                    };

                    List<ThingiverseImageDto> extraImages = null;
                    retryCount = 0;
                    while (retryCount < 3)
                    {
                        try
                        {
                            extraImages = await FetchItemImagesAsync(thing.Id);
                            break;
                        }
                        catch (HttpRequestException ex) when ((int?)ex.StatusCode == 429)
                        {
                            retryCount++;
                            await Task.Delay(3000);
                        }
                    }

                    if (extraImages != null)
                    {
                        foreach (var img in extraImages)
                        {
                            newThing.Images.Add(new ItemImage
                            {
                                ItemId = thing.Id,
                                ImageUrl = img.Url,
                                ThingiverseImageId = img.Id
                            });
                        }
                    }

                    await _thingRepository.AddAsync(newThing);
                }
            }

            await _thingRepository.SaveChangesAsync();
            await Task.Delay(1000); // rate limit
        }
    }



public async Task<ThingDetailDto> FetchThingDetailAsync(int thingId)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"https://api.thingiverse.com/things/{thingId}");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
        var res = await _httpClient.SendAsync(req);
        if (!res.IsSuccessStatusCode) return null;

        var json = await res.Content.ReadAsStringAsync();
        var detail = JsonConvert.DeserializeObject<ThingDetailDto>(json);
        return detail;
    }



    }
}






