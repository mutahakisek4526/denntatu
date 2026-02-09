using System.Collections.ObjectModel;
using AacV1.Core;
using AacV1.Core.Models;
using AacV1.Services;

namespace AacV1.VM;

public class MainViewModel : ObservableObject
{
    private readonly NavigationService _navigationService;
    private readonly InputService _inputService;
    private readonly DataStore _dataStore;
    private readonly SpeechService _speechService;
    private readonly PredictionService _predictionService;
    private readonly Settings _settings;

    private string _screenTitle = "ホーム";

    public MainViewModel(
        NavigationService navigationService,
        InputService inputService,
        DataStore dataStore,
        AppState appState,
        Settings settings,
        SpeechService speechService,
        PredictionService predictionService)
    {
        _navigationService = navigationService;
        _inputService = inputService;
        _dataStore = dataStore;
        _settings = settings;
        _speechService = speechService;
        _predictionService = predictionService;
        AppState = appState;

        NavigateCommand = new RelayCommand<string>(Navigate);
        SpeakCommand = new AsyncRelayCommand(() => _speechService.SpeakAsync(AppState.CurrentText));
        StopSpeakCommand = new RelayCommand(_ => _speechService.Stop());

        _inputService.HomeRequested += () => Navigate("Home");
        _inputService.SupporterRequested += () => Navigate("Supporter");
        _inputService.SelfTestRequested += () => Navigate("SelfTest");
        _inputService.SpeakRequested += () => _speechService.SpeakAsync(AppState.CurrentText);
        _inputService.StopSpeakRequested += () => _speechService.Stop();
        _inputService.ToggleDwellRequested += ToggleDwell;
        _inputService.ToggleTwoSwitchRequested += ToggleTwoSwitch;
    }

    public object? CurrentViewModel => _navigationService.CurrentViewModel;

    public AppState AppState { get; }

    public string ScreenTitle
    {
        get => _screenTitle;
        set => SetProperty(ref _screenTitle, value);
    }

    public string InputModeStatus => $"入力: {_settings.InputMode}";
    public string ScanStatus => $"走査: {_settings.ScanIntervalMs}ms";
    public string DwellStatus => $"ドウェル: {_settings.DwellFirstStageMs}/{_settings.DwellSecondStageMs}ms";
    public string TtsStatus => $"TTS: {_settings.SpeechVoice}";

    public RelayCommand<string> NavigateCommand { get; }
    public AsyncRelayCommand SpeakCommand { get; }
    public RelayCommand StopSpeakCommand { get; }

    public void RefreshPredictions()
    {
        var candidates = _predictionService.GetCandidates(AppState.CurrentText);
        AppState.Predictions = new ObservableCollection<string>(candidates);
    }

    public void LearnFromText(string text)
    {
        _predictionService.LearnFromText(text);
        _ = _dataStore.SaveDictionaryAsync(_predictionService.Entries.ToList());
    }

    public void SaveHistory()
    {
        _ = _dataStore.SaveHistoryAsync(AppState.History.ToList());
    }

    public void SaveFavorites()
    {
        _ = _dataStore.SaveFavoritesAsync(AppState.Favorites.ToList());
    }

    private void Navigate(string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }
        _navigationService.Navigate(key);
        ScreenTitle = key switch
        {
            "Home" => "ホーム",
            "Kana" => "50音文字盤",
            "Phrase" => "定型文",
            "History" => "履歴",
            "PcControl" => "PC操作",
            "Web" => "Web",
            "Mail" => "メール",
            "Env" => "環境制御",
            "Settings" => "設定",
            "Supporter" => "支援者",
            "SelfTest" => "セルフテスト",
            _ => "画面",
        };
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    private void ToggleDwell()
    {
        if (_settings.InputMode == InputMode.Dwell)
        {
            _settings.InputMode = InputMode.Keyboard;
        }
        else
        {
            _settings.InputMode = InputMode.Dwell;
        }
        _inputService.UpdateSettings(_settings);
        OnPropertyChanged(nameof(InputModeStatus));
    }

    private void ToggleTwoSwitch()
    {
        if (_settings.InputMode == InputMode.TwoSwitch)
        {
            _settings.InputMode = InputMode.Keyboard;
        }
        else
        {
            _settings.InputMode = InputMode.TwoSwitch;
        }
        _inputService.UpdateSettings(_settings);
        OnPropertyChanged(nameof(InputModeStatus));
    }

    public void HandlePreviewKeyDown(System.Windows.Input.KeyEventArgs e)
    {
        _inputService.HandlePreviewKeyDown(e);
    }
}
