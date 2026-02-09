using AacV1.Core;
using AacV1.Core.Models;
using AacV1.Services;

namespace AacV1.VM;

public class HistoryViewModel : ObservableObject, IViewLifecycle
{
    private readonly MainViewModel _mainViewModel;
    private readonly DataStore _dataStore;
    private HistoryEntry? _selected;

    public HistoryViewModel(MainViewModel mainViewModel, DataStore dataStore)
    {
        _mainViewModel = mainViewModel;
        _dataStore = dataStore;
        SpeakCommand = new AsyncRelayCommand(SpeakSelected);
        AddFavoriteCommand = new RelayCommand(_ => AddFavorite(), _ => Selected != null);
        RestoreCommand = new RelayCommand(_ => RestoreText(), _ => Selected != null);
    }

    public IEnumerable<HistoryEntry> Entries => _mainViewModel.AppState.History;

    public HistoryEntry? Selected
    {
        get => _selected;
        set => SetProperty(ref _selected, value);
    }

    public AsyncRelayCommand SpeakCommand { get; }
    public RelayCommand AddFavoriteCommand { get; }
    public RelayCommand RestoreCommand { get; }

    public void OnEnter() { }
    public void OnExit() { }

    private async Task SpeakSelected()
    {
        if (Selected == null)
        {
            return;
        }
        _mainViewModel.AppState.CurrentText = Selected.Text;
        _mainViewModel.SpeakCommand.Execute(null);
        await Task.CompletedTask;
    }

    private void AddFavorite()
    {
        if (Selected == null)
        {
            return;
        }
        _mainViewModel.AppState.Favorites.Add(new FavoriteEntry { Text = Selected.Text });
        _mainViewModel.SaveFavorites();
    }

    private void RestoreText()
    {
        if (Selected == null)
        {
            return;
        }
        _mainViewModel.AppState.CurrentText = Selected.Text;
    }
}
