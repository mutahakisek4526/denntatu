using System.IO.Compression;
using AacV1.Core;
using AacV1.Services;

namespace AacV1.VM;

public class SupporterViewModel : ObservableObject
{
    private readonly MainViewModel _mainViewModel;
    private readonly DataStore _dataStore;
    private string _logText = string.Empty;

    public SupporterViewModel(MainViewModel mainViewModel, DataStore dataStore)
    {
        _mainViewModel = mainViewModel;
        _dataStore = dataStore;
        BackupCommand = new AsyncRelayCommand(BackupAsync);
        RestoreCommand = new AsyncRelayCommand(RestoreAsync);
        RefreshLogCommand = new RelayCommand(_ => LogText = _dataStore.ReadLogs());
        LogText = _dataStore.ReadLogs();
    }

    public string LogText
    {
        get => _logText;
        set => SetProperty(ref _logText, value);
    }

    public AsyncRelayCommand BackupCommand { get; }
    public AsyncRelayCommand RestoreCommand { get; }
    public RelayCommand RefreshLogCommand { get; }

    private async Task BackupAsync()
    {
        var backupPath = Path.Combine(AppDataPaths.Root, "backup.zip");
        if (File.Exists(backupPath))
        {
            File.Delete(backupPath);
        }
        ZipFile.CreateFromDirectory(AppDataPaths.Root, backupPath);
        await Task.CompletedTask;
    }

    private async Task RestoreAsync()
    {
        var backupPath = Path.Combine(AppDataPaths.Root, "backup.zip");
        if (!File.Exists(backupPath))
        {
            return;
        }
        ZipFile.ExtractToDirectory(backupPath, AppDataPaths.Root, true);
        await Task.CompletedTask;
    }
}
