using System.Windows;
using System.Windows.Controls;
using AacV1.VM;

namespace AacV1.Views;

public partial class SelfTestView : UserControl
{
    public SelfTestView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ScanDwellHelper.Register(this);
        if (DataContext is SelfTestViewModel vm)
        {
            var point = PcTargetButton.PointToScreen(new Point(PcTargetButton.ActualWidth / 2, PcTargetButton.ActualHeight / 2));
            vm.SetPcTargetPosition(point);
        }
    }

    private void OnPcTargetClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is SelfTestViewModel vm)
        {
            vm.MarkPcTestClicked();
        }
    }
}
