using System.Windows.Controls;

namespace AacV1.Views;

public partial class SupporterView : UserControl
{
    public SupporterView()
    {
        InitializeComponent();
        Loaded += (_, _) => ScanDwellHelper.Register(this);
    }
}
