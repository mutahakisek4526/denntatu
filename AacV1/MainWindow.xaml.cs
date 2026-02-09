using System.Windows;
using System.Windows.Input;
using AacV1.VM;

namespace AacV1;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        PreviewKeyDown += OnPreviewKeyDown;
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.HandlePreviewKeyDown(e);
        }
    }
}
