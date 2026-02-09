using AacV1.Core;

namespace AacV1.VM;

public class WebViewModel : ObservableObject
{
    private string _url = "https://www.example.com";

    public WebViewModel(MainViewModel mainViewModel)
    {
        NavigateCommand = new RelayCommand(_ => NavigateRequested?.Invoke(_url));
        BackCommand = new RelayCommand(_ => BackRequested?.Invoke());
        ForwardCommand = new RelayCommand(_ => ForwardRequested?.Invoke());
        RefreshCommand = new RelayCommand(_ => RefreshRequested?.Invoke());
        HomeCommand = new RelayCommand(_ =>
        {
            Url = "https://www.example.com";
            NavigateRequested?.Invoke(_url);
        });
    }

    public string Url
    {
        get => _url;
        set => SetProperty(ref _url, value);
    }

    public RelayCommand NavigateCommand { get; }
    public RelayCommand BackCommand { get; }
    public RelayCommand ForwardCommand { get; }
    public RelayCommand RefreshCommand { get; }
    public RelayCommand HomeCommand { get; }

    public event Action<string>? NavigateRequested;
    public event Action? BackRequested;
    public event Action? ForwardRequested;
    public event Action? RefreshRequested;
}
