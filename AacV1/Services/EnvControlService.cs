using System.Net.Http;
using System.Text;
using AacV1.Core.Models;

namespace AacV1.Services;

public class EnvControlService
{
    private readonly HttpClient _client = new();

    public async Task<string> SendAsync(EnvDevice device, CancellationToken token = default)
    {
        using var request = new HttpRequestMessage(new HttpMethod(device.Method), device.Url);
        foreach (var header in device.Headers)
        {
            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (!string.IsNullOrWhiteSpace(device.Body))
        {
            request.Content = new StringContent(device.Body, Encoding.UTF8, "application/json");
        }

        using var response = await _client.SendAsync(request, token).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
        return $"{(int)response.StatusCode} {response.ReasonPhrase} {content}";
    }
}
