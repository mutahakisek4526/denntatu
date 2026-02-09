using System.Windows.Controls;
using AacV1.VM;

namespace AacV1.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        Loaded += (_, _) => ScanDwellHelper.Register(this);
    }

    private void PasswordBox_OnPasswordChanged(object sender, System.Windows.RoutedEventArgs e)
    {
        if (DataContext is SettingsViewModel vm && sender is PasswordBox box)
        {
            vm.Settings.Smtp.Password = box.Password;
        }
    }
}
