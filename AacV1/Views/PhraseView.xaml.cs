using System.Windows;
using System.Windows.Controls;
using AacV1.VM;

namespace AacV1.Views;

public partial class PhraseView : UserControl
{
    public PhraseView()
    {
        InitializeComponent();
        Loaded += (_, _) => ScanDwellHelper.Register(this);
    }

    private void OnInsertPhrase(object sender, RoutedEventArgs e)
    {
        if (DataContext is PhraseViewModel vm)
        {
            vm.InsertPhraseToInput();
        }
    }
}
