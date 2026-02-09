using System.Collections.ObjectModel;
using AacV1.Core;
using AacV1.Core.Models;
using AacV1.Services;

namespace AacV1.VM;

public class PhraseViewModel : ObservableObject, IViewLifecycle
{
    private readonly MainViewModel _mainViewModel;
    private readonly DataStore _dataStore;
    private PhraseCategory? _selectedCategory;
    private PhraseItem? _selectedPhrase;
    private string _newCategoryName = string.Empty;
    private string _newPhraseText = string.Empty;

    public PhraseViewModel(MainViewModel mainViewModel, DataStore dataStore, List<PhraseCategory> categories)
    {
        _mainViewModel = mainViewModel;
        _dataStore = dataStore;
        Categories = new ObservableCollection<PhraseCategory>(categories);
        AddCategoryCommand = new RelayCommand(_ => AddCategory());
        AddPhraseCommand = new RelayCommand(_ => AddPhrase(), _ => SelectedCategory != null);
        DeletePhraseCommand = new RelayCommand(_ => DeletePhrase(), _ => SelectedPhrase != null);
        SpeakPhraseCommand = new AsyncRelayCommand(() => _mainViewModel.SpeakCommand.Execute(null));
    }

    public ObservableCollection<PhraseCategory> Categories { get; }

    public PhraseCategory? SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    public PhraseItem? SelectedPhrase
    {
        get => _selectedPhrase;
        set => SetProperty(ref _selectedPhrase, value);
    }

    public string NewCategoryName
    {
        get => _newCategoryName;
        set => SetProperty(ref _newCategoryName, value);
    }

    public string NewPhraseText
    {
        get => _newPhraseText;
        set => SetProperty(ref _newPhraseText, value);
    }

    public RelayCommand AddCategoryCommand { get; }
    public RelayCommand AddPhraseCommand { get; }
    public RelayCommand DeletePhraseCommand { get; }
    public AsyncRelayCommand SpeakPhraseCommand { get; }

    public void OnEnter() { }
    public void OnExit() => Save();

    public void InsertPhraseToInput()
    {
        if (SelectedPhrase == null)
        {
            return;
        }
        _mainViewModel.AppState.CurrentText = SelectedPhrase.Text;
    }

    private void AddCategory()
    {
        if (string.IsNullOrWhiteSpace(NewCategoryName))
        {
            return;
        }
        var category = new PhraseCategory { Name = NewCategoryName };
        Categories.Add(category);
        SelectedCategory = category;
        NewCategoryName = string.Empty;
        Save();
    }

    private void AddPhrase()
    {
        if (SelectedCategory == null || string.IsNullOrWhiteSpace(NewPhraseText))
        {
            return;
        }
        SelectedCategory.Items.Add(new PhraseItem { Text = NewPhraseText });
        NewPhraseText = string.Empty;
        Save();
    }

    private void DeletePhrase()
    {
        if (SelectedCategory == null || SelectedPhrase == null)
        {
            return;
        }
        SelectedCategory.Items.Remove(SelectedPhrase);
        SelectedPhrase = null;
        Save();
    }

    private void Save() => _ = _dataStore.SavePhrasesAsync(Categories.ToList());
}
