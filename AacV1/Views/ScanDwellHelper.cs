using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AacV1.Services;

namespace AacV1.Views;

public static class ScanDwellHelper
{
    private static List<Button> _currentButtons = new();

    public static void Register(UserControl control)
    {
        var scanService = ServiceLocator.ScanService;
        var dwellService = ServiceLocator.DwellService;
        if (scanService == null || dwellService == null)
        {
            return;
        }

        foreach (var button in _currentButtons)
        {
            button.MouseEnter -= OnMouseEnter;
            button.MouseLeave -= OnMouseLeave;
        }

        _currentButtons = FindVisualChildren<Button>(control).ToList();
        scanService.SetTargets(_currentButtons.Select<Button, Action>(button => () => button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent))));
        scanService.IndexChanged -= OnIndexChanged;
        scanService.IndexChanged += OnIndexChanged;

        for (var i = 0; i < _currentButtons.Count; i++)
        {
            var localId = i;
            _currentButtons[i].Tag = localId;
            _currentButtons[i].MouseEnter += OnMouseEnter;
            _currentButtons[i].MouseLeave += OnMouseLeave;
        }

        dwellService.StageTwo -= OnStageTwo;
        dwellService.StageTwo += OnStageTwo;

        void OnIndexChanged(int index)
        {
            if (index >= 0 && index < _currentButtons.Count)
            {
                _currentButtons[index].Focus();
            }
        }

        void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                dwellService.HoverEnter(id);
            }
        }

        void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                dwellService.HoverLeave(id);
            }
        }

        void OnStageTwo(int id)
        {
            if (id >= 0 && id < _currentButtons.Count)
            {
                _currentButtons[id].RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }

    private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
        if (depObj == null)
        {
            yield break;
        }

        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        {
            var child = VisualTreeHelper.GetChild(depObj, i);
            if (child is T t)
            {
                yield return t;
            }

            foreach (var childOfChild in FindVisualChildren<T>(child))
            {
                yield return childOfChild;
            }
        }
    }
}
