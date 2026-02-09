using System.Text.RegularExpressions;
using AacV1.Core.Models;

namespace AacV1.Services;

public class PredictionService
{
    private readonly List<DictionaryEntry> _entries;

    public PredictionService(List<DictionaryEntry> entries)
    {
        _entries = entries;
    }

    public IReadOnlyList<DictionaryEntry> Entries => _entries;

    public IReadOnlyList<string> GetCandidates(string text, int max = 5)
    {
        var token = ExtractLastToken(text);
        if (string.IsNullOrWhiteSpace(token))
        {
            return _entries.OrderByDescending(e => e.Count).Select(e => e.Word).Take(max).ToList();
        }

        return _entries
            .Where(e => e.Word.StartsWith(token, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(e => e.Count)
            .Select(e => e.Word)
            .Take(max)
            .ToList();
    }

    public void LearnFromText(string text)
    {
        foreach (var word in SplitWords(text))
        {
            var entry = _entries.FirstOrDefault(e => e.Word == word);
            if (entry == null)
            {
                _entries.Add(new DictionaryEntry { Word = word, Count = 1 });
            }
            else
            {
                entry.Count++;
            }
        }
    }

    private static string ExtractLastToken(string text)
    {
        var tokens = SplitWords(text).ToList();
        return tokens.LastOrDefault() ?? string.Empty;
    }

    private static IEnumerable<string> SplitWords(string text)
    {
        foreach (var token in Regex.Split(text, "[\u3000\s,。.、！!？?]+"))
        {
            var trimmed = token.Trim();
            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                yield return trimmed;
            }
        }
    }
}
