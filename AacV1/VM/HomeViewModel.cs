using AacV1.Core;

namespace AacV1.VM;

public class HomeViewModel : ObservableObject
{
    private readonly MainViewModel _mainViewModel;

    public HomeViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        NavigateCommand = _mainViewModel.NavigateCommand;
    }

    public RelayCommand<string> NavigateCommand { get; }
}
