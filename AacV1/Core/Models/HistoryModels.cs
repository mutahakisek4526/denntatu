namespace AacV1.Core.Models;

public class HistoryEntry
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string Text { get; set; } = string.Empty;
}

public class FavoriteEntry
{
    public string Text { get; set; } = string.Empty;
    public DateTime AddedAt { get; set; } = DateTime.Now;
}
