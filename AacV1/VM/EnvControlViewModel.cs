using System.Collections.ObjectModel;
using AacV1.Core;
using AacV1.Core.Models;
using AacV1.Services;

namespace AacV1.VM;

public class EnvControlViewModel : ObservableObject, IViewLifecycle
{
    private readonly MainViewModel _mainViewModel;
    private readonly DataStore _dataStore;
    private readonly EnvControlService _envService;
    private EnvDevice? _selected;
    private string _result = string.Empty;

    public EnvControlViewModel(MainViewModel mainViewModel, DataStore dataStore, EnvControlService envService, List<EnvDevice> devices)
    {
        _mainViewModel = mainViewModel;
        _dataStore = dataStore;
        _envService = envService;
        Devices = new ObservableCollection<EnvDevice>(devices);
        AddDeviceCommand = new RelayCommand(_ => AddDevice());
        SendCommand = new AsyncRelayCommand(SendAsync);
        SaveCommand = new RelayCommand(_ => Save());
    }

    public ObservableCollection<EnvDevice> Devices { get; }

    public EnvDevice? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public string Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }

    public RelayCommand AddDeviceCommand { get; }
    public AsyncRelayCommand SendCommand { get; }
    public RelayCommand SaveCommand { get; }

    public void OnEnter() { }
    public void OnExit() => Save();

    private void AddDevice()
    {
        var device = new EnvDevice { Name = "新規デバイス", Url = "http://" };
        Devices.Add(device);
        Selected = device;
        Save();
    }

    private async Task SendAsync()
    {
        if (Selected == null)
        {
            return;
        }
        try
        {
            Result = await _envService.SendAsync(Selected);
        }
        catch (Exception ex)
        {
            Result = $"送信失敗: {ex.Message}";
        }
    }

    private void Save() => _ = _dataStore.SaveEnvAsync(Devices.ToList());
}
