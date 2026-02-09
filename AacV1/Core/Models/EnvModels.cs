namespace AacV1.Core.Models;

public class EnvDevice
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = "GET";
    public string Body { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = new();
}
