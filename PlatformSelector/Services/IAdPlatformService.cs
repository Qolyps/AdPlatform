using Microsoft.AspNetCore.Http;

namespace AdPlatformSelector.Services;

public interface IAdPlatformService
{
    Task LoadFromFileAsync(IFormFile file);
    List<string> FindPlatformsByLocation(string location);
}
