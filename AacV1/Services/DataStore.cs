using AacV1.Core;
using AacV1.Core.Models;

namespace AacV1.Services;

public class DataStore
{
    private readonly JsonStorage _storage;
    private readonly Logging _logging;

    public DataStore(JsonStorage storage, Logging logging)
    {
        _storage = storage;
        _logging = logging;
    }

    public async Task<Settings> LoadSettingsAsync(CancellationToken token = default) =>
        await _storage.LoadAsync(AppDataPaths.Settings, new Settings(), token).ConfigureAwait(false);

    public async Task SaveSettingsAsync(Settings settings, CancellationToken token = default) =>
        await _storage.SaveAsync(AppDataPaths.Settings, settings, token).ConfigureAwait(false);

    public async Task<List<PhraseCategory>> LoadPhrasesAsync(CancellationToken token = default) =>
        await _storage.LoadAsync(AppDataPaths.Phrases, new List<PhraseCategory>(), token).ConfigureAwait(false);

    public async Task SavePhrasesAsync(List<PhraseCategory> categories, CancellationToken token = default) =>
        await _storage.SaveAsync(AppDataPaths.Phrases, categories, token).ConfigureAwait(false);

    public async Task<List<HistoryEntry>> LoadHistoryAsync(CancellationToken token = default) =>
        await _storage.LoadAsync(AppDataPaths.History, new List<HistoryEntry>(), token).ConfigureAwait(false);

    public async Task SaveHistoryAsync(List<HistoryEntry> entries, CancellationToken token = default) =>
        await _storage.SaveAsync(AppDataPaths.History, entries, token).ConfigureAwait(false);

    public async Task<List<FavoriteEntry>> LoadFavoritesAsync(CancellationToken token = default) =>
        await _storage.LoadAsync(AppDataPaths.Favorites, new List<FavoriteEntry>(), token).ConfigureAwait(false);

    public async Task SaveFavoritesAsync(List<FavoriteEntry> entries, CancellationToken token = default) =>
        await _storage.SaveAsync(AppDataPaths.Favorites, entries, token).ConfigureAwait(false);

    public async Task<List<DictionaryEntry>> LoadDictionaryAsync(CancellationToken token = default) =>
        await _storage.LoadAsync(AppDataPaths.Dictionary, new List<DictionaryEntry>(), token).ConfigureAwait(false);

    public async Task SaveDictionaryAsync(List<DictionaryEntry> entries, CancellationToken token = default) =>
        await _storage.SaveAsync(AppDataPaths.Dictionary, entries, token).ConfigureAwait(false);

    public async Task<List<EnvDevice>> LoadEnvAsync(CancellationToken token = default) =>
        await _storage.LoadAsync(AppDataPaths.Env, new List<EnvDevice>(), token).ConfigureAwait(false);

    public async Task SaveEnvAsync(List<EnvDevice> devices, CancellationToken token = default) =>
        await _storage.SaveAsync(AppDataPaths.Env, devices, token).ConfigureAwait(false);

    public void LogInfo(string message) => _logging.Info(message);
    public void LogError(string message, Exception? ex = null) => _logging.Error(message, ex);
    public string ReadLogs() => _logging.ReadAll();
}
