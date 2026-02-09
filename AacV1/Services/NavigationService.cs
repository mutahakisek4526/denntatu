using AacV1.Core;

namespace AacV1.Services;

public interface IViewLifecycle
{
    void OnEnter();
    void OnExit();
}

public class NavigationService : ObservableObject
{
    private readonly Dictionary<string, Func<object>> _factory = new();
    private object? _currentViewModel;

    public object? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public void Register(string key, Func<object> factory)
    {
        _factory[key] = factory;
    }

    public void Navigate(string key)
    {
        if (!_factory.TryGetValue(key, out var factory))
        {
            return;
        }

        if (_currentViewModel is IViewLifecycle lifecycle)
        {
            lifecycle.OnExit();
        }

        ServiceLocator.ScanService?.Stop();
        ServiceLocator.DwellService?.Reset();

        CurrentViewModel = factory();

        if (_currentViewModel is IViewLifecycle newLifecycle)
        {
            newLifecycle.OnEnter();
        }
    }
}
