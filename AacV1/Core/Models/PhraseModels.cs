namespace AacV1.Core.Models;

public class PhraseCategory
{
    public string Name { get; set; } = string.Empty;
    public List<PhraseItem> Items { get; set; } = new();
}

public class PhraseItem
{
    public string Text { get; set; } = string.Empty;
}
