using System.Windows.Controls;

namespace AacV1.Views;

public partial class PcControlView : UserControl
{
    public PcControlView()
    {
        InitializeComponent();
        Loaded += (_, _) => ScanDwellHelper.Register(this);
    }
}
