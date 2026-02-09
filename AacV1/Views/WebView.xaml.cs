using System;
using System.Windows.Controls;
using AacV1.VM;

namespace AacV1.Views;

public partial class WebView : UserControl
{
    public WebView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
        Loaded += (_, _) => ScanDwellHelper.Register(this);
    }

    private async void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is not WebViewModel vm)
        {
            return;
        }

        await Browser.EnsureCoreWebView2Async();
        vm.NavigateRequested += url => Browser.Source = new Uri(url);
        vm.BackRequested += () =>
        {
            if (Browser.CanGoBack)
            {
                Browser.GoBack();
            }
        };
        vm.ForwardRequested += () =>
        {
            if (Browser.CanGoForward)
            {
                Browser.GoForward();
            }
        };
        vm.RefreshRequested += () => Browser.Reload();
        vm.NavigateCommand.Execute(null);
    }
}
