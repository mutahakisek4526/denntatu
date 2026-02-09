using System.Collections.ObjectModel;
using AacV1.Core;
using AacV1.Core.Models;
using AacV1.Services;

namespace AacV1.VM;

public class SettingsViewModel : ObservableObject, IViewLifecycle
{
    private readonly MainViewModel _mainViewModel;
    private readonly DataStore _dataStore;
    private readonly SpeechService _speechService;
    private readonly ScanService _scanService;
    private readonly DwellService _dwellService;
    private readonly InputService _inputService;

    public SettingsViewModel(
        MainViewModel mainViewModel,
        DataStore dataStore,
        Settings settings,
        SpeechService speechService,
        ScanService scanService,
        DwellService dwellService,
        InputService inputService)
    {
        _mainViewModel = mainViewModel;
        _dataStore = dataStore;
        Settings = settings;
        _speechService = speechService;
        _scanService = scanService;
        _dwellService = dwellService;
        _inputService = inputService;
        Voices = new ObservableCollection<string>(_speechService.GetVoices());
        SaveCommand = new RelayCommand(_ => Save());
    }

    public Settings Settings { get; }
    public ObservableCollection<string> Voices { get; }

    public RelayCommand SaveCommand { get; }

    public void OnEnter() { }
    public void OnExit() => Save();

    private void Save()
    {
        _speechService.ApplySettings(Settings);
        _scanService.UpdateInterval(Settings.ScanIntervalMs);
        _dwellService.UpdateTimings(Settings.DwellFirstStageMs, Settings.DwellSecondStageMs);
        _inputService.UpdateSettings(Settings);
        if (Settings.InputMode == InputMode.AutoScan)
        {
            _scanService.Start();
        }
        else
        {
            _scanService.Stop();
        }
        _ = _dataStore.SaveSettingsAsync(Settings);
    }
}
