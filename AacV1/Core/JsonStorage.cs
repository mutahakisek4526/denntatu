using System.Text.Json;

namespace AacV1.Core;

public class JsonStorage
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task SaveAsync<T>(string path, T data, CancellationToken token = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await using var stream = File.Create(path);
        await JsonSerializer.SerializeAsync(stream, data, _options, token).ConfigureAwait(false);
    }

    public async Task<T> LoadAsync<T>(string path, T fallback, CancellationToken token = default)
    {
        if (!File.Exists(path))
        {
            return fallback;
        }

        await using var stream = File.OpenRead(path);
        var result = await JsonSerializer.DeserializeAsync<T>(stream, _options, token).ConfigureAwait(false);
        return result ?? fallback;
    }
}
