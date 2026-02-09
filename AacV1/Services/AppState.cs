using System.Collections.ObjectModel;
using AacV1.Core;
using AacV1.Core.Models;

namespace AacV1.Services;

public class AppState : ObservableObject
{
    private string _currentText = string.Empty;
    private string _statusMessage = "準備完了";
    private ObservableCollection<string> _predictions = new();

    public string CurrentText
    {
        get => _currentText;
        set => SetProperty(ref _currentText, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public ObservableCollection<string> Predictions
    {
        get => _predictions;
        set => SetProperty(ref _predictions, value);
    }

    public ObservableCollection<HistoryEntry> History { get; } = new();
    public ObservableCollection<FavoriteEntry> Favorites { get; } = new();
}
