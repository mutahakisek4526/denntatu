using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using AacV1.Core;
using AacV1.Core.Models;

namespace AacV1.Services;

public class InputService : ObservableObject
{
    private Settings _settings;
    private readonly ScanService _scanService;
    private readonly DwellService _dwellService;

    public InputService(Settings settings, ScanService scanService, DwellService dwellService)
    {
        _settings = settings;
        _scanService = scanService;
        _dwellService = dwellService;
    }

    public event Action? HomeRequested;
    public event Action? SupporterRequested;
    public event Action? SelfTestRequested;
    public event Action? BackRequested;
    public event Action? SpeakRequested;
    public event Action? StopSpeakRequested;
    public event Action? ToggleDwellRequested;
    public event Action? ToggleTwoSwitchRequested;

    public void UpdateSettings(Settings settings)
    {
        _settings = settings;
    }

    public void HandlePreviewKeyDown(KeyEventArgs e)
    {
        if (e.Key == Key.F1)
        {
            SupporterRequested?.Invoke();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.F2)
        {
            HomeRequested?.Invoke();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.F3)
        {
            SelfTestRequested?.Invoke();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.F5)
        {
            if (_scanService.IsRunning)
            {
                _scanService.Stop();
            }
            else
            {
                _scanService.Start();
            }
            e.Handled = true;
            return;
        }

        if (e.Key == Key.F6)
        {
            SpeakRequested?.Invoke();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.F7)
        {
            StopSpeakRequested?.Invoke();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.F8)
        {
            ToggleDwellRequested?.Invoke();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.F9)
        {
            ToggleTwoSwitchRequested?.Invoke();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Escape)
        {
            BackRequested?.Invoke();
            e.Handled = true;
            return;
        }

        if (_settings.InputMode == InputMode.TwoSwitch)
        {
            if (e.Key.ToString() == _settings.TwoSwitchNextKey)
            {
                _scanService.MoveNext();
                e.Handled = true;
                return;
            }

            if (e.Key.ToString() == _settings.TwoSwitchSelectKey)
            {
                _scanService.Select();
                e.Handled = true;
                return;
            }
        }

        if (_settings.InputMode == InputMode.Keyboard)
        {
            if (e.Key is Key.Up or Key.Down or Key.Left or Key.Right)
            {
                var direction = e.Key switch
                {
                    Key.Up => FocusNavigationDirection.Up,
                    Key.Down => FocusNavigationDirection.Down,
                    Key.Left => FocusNavigationDirection.Left,
                    Key.Right => FocusNavigationDirection.Right,
                    _ => FocusNavigationDirection.Next
                };
                (Keyboard.FocusedElement as UIElement)?.MoveFocus(new TraversalRequest(direction));
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                if (Keyboard.FocusedElement is Button button)
                {
                    button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    e.Handled = true;
                    return;
                }
            }
        }
    }
}
