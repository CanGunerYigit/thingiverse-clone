using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Thingiverse.Application.Contracts.DTO ;
using Thingiverse.Application.Interfaces;
[ApiController]
[Route("api/[controller]")]
public class MakesController : ControllerBase
{
    private readonly IMakeRepository _makeRepository;

    public MakesController(IMakeRepository makeRepository)
    {
        _makeRepository = makeRepository;
    }

    [HttpPost("{itemId}/makes")]
    [Authorize]
    public async Task<IActionResult> CreateMake(int itemId, [FromForm] ThingMakeDto dto)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized("Kullanıcı doğrulanamadı.");

        try
        {
            var make = await _makeRepository.CreateMakeAsync(itemId, dto, userId);
            return Ok(make);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("item/{itemId}")]
    [Authorize]
    public async Task<IActionResult> GetMakesByItemId(int itemId)
    {
        var makesDto = await _makeRepository.GetMakesByItemIdAsync(itemId);
        return Ok(makesDto);
    }

    [HttpGet("most-makes")]
    public async Task<IActionResult> GetItemsByMostMakes()
    {
        var result = await _makeRepository.GetItemsByMostMakesAsync();
        return Ok(result);
    }

    [HttpGet("{makeId}")]
    public async Task<IActionResult> GetMakeById(int makeId)
    {
        var make = await _makeRepository.GetMakeByIdAsync(makeId);
        if (make == null)
            return NotFound("Make bulunamadı.");

        return Ok(make);
    }
}
