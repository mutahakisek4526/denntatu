using System.Windows.Controls;

namespace AacV1.Views;

public partial class EnvControlView : UserControl
{
    public EnvControlView()
    {
        InitializeComponent();
        Loaded += (_, _) => ScanDwellHelper.Register(this);
    }
}
