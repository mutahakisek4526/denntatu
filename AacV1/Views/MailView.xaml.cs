using System.Windows.Controls;

namespace AacV1.Views;

public partial class MailView : UserControl
{
    public MailView()
    {
        InitializeComponent();
        Loaded += (_, _) => ScanDwellHelper.Register(this);
    }
}
