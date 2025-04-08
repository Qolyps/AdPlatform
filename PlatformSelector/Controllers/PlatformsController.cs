using Microsoft.AspNetCore.Mvc;
using AdPlatformSelector.Services;

namespace AdPlatformSelector.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IAdPlatformService _adPlatformService;

    public PlatformsController(IAdPlatformService adPlatformService)
    {
        _adPlatformService = adPlatformService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Файл не загружен.");

        try
        {
            await _adPlatformService.LoadFromFileAsync(file);
            return Ok("Файл загружен и данные обновлены.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Ошибка при обработке файла: {ex.Message}");
        }
    }

    [HttpGet("search")]
    public IActionResult SearchByLocation([FromQuery] string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return BadRequest("Локация не указана.");

        var platforms = _adPlatformService.FindPlatformsByLocation(location);
        if (platforms.Count == 0)
        {
            return NotFound("Нет рекламных площадок для данной локации.");
        }

        return Ok(platforms);
    }
}
