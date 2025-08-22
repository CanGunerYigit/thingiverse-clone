using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Thingiverse.Application.Contracts.DTO.Popular;
using Thingiverse.Application.Contracts.DTO;
using Thingiverse.Application.Contracts.Repository;
using Thingiverse.Application.Options;
using Thingiverse.Domain.Models;
using Thingiverse.Application.Contracts.Config;

public class ThingiverseService
{
    private readonly HttpClient _httpClient;
    private readonly IThingRepository _thingRepository;
    private readonly ApiSettings _apiSettings;
    private readonly string _accessToken;

    public ThingiverseService(
        HttpClient httpClient, IThingRepository thingRepository, IOptions<ThingiverseOptions> options, IOptions<ApiSettings> apiOptions)
    {
        _httpClient = httpClient;
        _thingRepository = thingRepository;
        _accessToken = options.Value.Token;
        _apiSettings = apiOptions.Value;

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _accessToken);
    }

    public async Task<List<ThingiverseImageDto>> FetchItemImagesAsync(int thingId)
    {
        string url = $"{_apiSettings.ThingiverseBaseUrl}/things/{thingId}/images";
        var images = await _httpClient.GetFromJsonAsync<List<ThingiverseImageDto>>(url);
        return images ?? new List<ThingiverseImageDto>();
    }

    public async Task FetchAndSaveAllPopularThingsAsync(string filter, int totalPages = 50, int perPage = 20)
    {
        for (int page = 1; page <= totalPages; page++)
        {
            string url = $"{_apiSettings.ThingiverseBaseUrl}/popular?filter={filter}&page={page}&per_page={perPage}";

            List<PopularApiThingDtoAllTime>? things = null;
            int retryCount = 0;

            while (retryCount < 3)
            {
                try
                {
                    things = await _httpClient.GetFromJsonAsync<List<PopularApiThingDtoAllTime>>(url);
                    break;
                }
                catch (HttpRequestException)
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

                    List<ThingiverseImageDto>? extraImages = null;
                    retryCount = 0;
                    while (retryCount < 3)
                    {
                        try
                        {
                            extraImages = await FetchItemImagesAsync(thing.Id);
                            break;
                        }
                        catch (HttpRequestException)
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
            await Task.Delay(1000); // Rate limit için bekleme
        }
    }

    public async Task<ThingDetailDto?> FetchThingDetailAsync(int thingId)
    {
        var res = await _httpClient.GetAsync($"{_apiSettings.ThingiverseBaseUrl}/things/{thingId}");
        if (!res.IsSuccessStatusCode) return null;

        var json = await res.Content.ReadAsStringAsync();
        var detail = JsonConvert.DeserializeObject<ThingDetailDto>(json);
        return detail;
    }
}
