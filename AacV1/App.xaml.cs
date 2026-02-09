using System.Windows;
using System.Windows.Threading;
using AacV1.Core;
using AacV1.Core.Models;
using AacV1.Services;
using AacV1.VM;

namespace AacV1;

public partial class App : Application
{
    private Logging? _logging;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _logging = new Logging(AppDataPaths.Log);
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

        var storage = new JsonStorage();
        var dataStore = new DataStore(storage, _logging);
        var settings = await dataStore.LoadSettingsAsync();
        var phrases = await dataStore.LoadPhrasesAsync();
        var history = await dataStore.LoadHistoryAsync();
        var favorites = await dataStore.LoadFavoritesAsync();
        var dictionary = await dataStore.LoadDictionaryAsync();
        var env = await dataStore.LoadEnvAsync();

        var scanService = new ScanService(settings.ScanIntervalMs);
        var dwellService = new DwellService(settings.DwellFirstStageMs, settings.DwellSecondStageMs);
        var inputService = new InputService(settings, scanService, dwellService);
        var speechService = new SpeechService(settings);
        var predictionService = new PredictionService(dictionary);
        var appState = new AppState();
        foreach (var entry in history)
        {
            appState.History.Add(entry);
        }
        foreach (var entry in favorites)
        {
            appState.Favorites.Add(entry);
        }

        var navigationService = new NavigationService();

        ServiceLocator.ScanService = scanService;
        ServiceLocator.DwellService = dwellService;

        if (settings.InputMode == InputMode.AutoScan)
        {
            scanService.Start();
        }

        var mainViewModel = new MainViewModel(navigationService, inputService, dataStore, appState, settings, speechService, predictionService);

        navigationService.Register("Home", () => new HomeViewModel(mainViewModel));
        navigationService.Register("Kana", () => new KanaBoardViewModel(mainViewModel));
        navigationService.Register("Phrase", () => new PhraseViewModel(mainViewModel, dataStore, phrases));
        navigationService.Register("History", () => new HistoryViewModel(mainViewModel, dataStore));
        navigationService.Register("PcControl", () => new PcControlViewModel(mainViewModel, new PcControlService(_logging)));
        navigationService.Register("Web", () => new WebViewModel(mainViewModel));
        navigationService.Register("Mail", () => new MailViewModel(mainViewModel, new MailService(settings)));
        navigationService.Register("Env", () => new EnvControlViewModel(mainViewModel, dataStore, new EnvControlService(), env));
        navigationService.Register("Settings", () => new SettingsViewModel(mainViewModel, dataStore, settings, speechService, scanService, dwellService, inputService));
        navigationService.Register("Supporter", () => new SupporterViewModel(mainViewModel, dataStore));
        navigationService.Register("SelfTest", () => new SelfTestViewModel(mainViewModel, dataStore, speechService, scanService, dwellService, new EnvControlService(), new PcControlService(_logging)));

        navigationService.Navigate("Home");

        var window = new MainWindow
        {
            DataContext = mainViewModel
        };
        window.Show();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        _logging?.Error("UI例外", e.Exception);
        e.Handled = true;
    }

    private void OnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            _logging?.Error("致命的例外", ex);
        }
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        _logging?.Error("未観測タスク例外", e.Exception);
        e.SetObserved();
    }
}
