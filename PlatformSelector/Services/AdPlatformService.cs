using System.Text;
using AdPlatformSelector.Models;
using Microsoft.AspNetCore.Http;

namespace AdPlatformSelector.Services;

public class AdPlatformService : IAdPlatformService
{
    private readonly Dictionary<string, List<string>> _locationPlatformMap = new();

    public async Task LoadFromFileAsync(IFormFile file)
    {
        _locationPlatformMap.Clear();

        using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line) || !line.Contains(":")) continue;

            var parts = line.Split(':', 2);
            var name = parts[0].Trim();
            var locations = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(x => x.Trim())
                                    .ToList();

            if (string.IsNullOrEmpty(name) || locations.Count == 0)
            {
                throw new InvalidOperationException("Неверный формат данных в файле.");
            }

            foreach (var location in locations)
            {
                var key = NormalizeLocation(location);
                if (!_locationPlatformMap.ContainsKey(key))
                {
                    _locationPlatformMap[key] = new List<string>();
                }

                _locationPlatformMap[key].Add(name);
            }
        }
    }

    public List<string> FindPlatformsByLocation(string location)
    {
        if (string.IsNullOrWhiteSpace(location)) return new();

        var result = new HashSet<string>();
        var parts = NormalizeLocation(location).Split('/', StringSplitOptions.RemoveEmptyEntries);

        for (int i = parts.Length; i >= 1; i--)
        {
            var prefix = "/" + string.Join('/', parts.Take(i));
            if (_locationPlatformMap.TryGetValue(prefix, out var platforms))
            {
                foreach (var p in platforms)
                    result.Add(p);
            }
        }

        if (_locationPlatformMap.TryGetValue("/", out var rootPlatforms))
        {
            foreach (var p in rootPlatforms)
                result.Add(p);
        }

        return result.ToList();
    }

    private static string NormalizeLocation(string location)
        => location.Trim().ToLower().Replace("//", "/");
}
