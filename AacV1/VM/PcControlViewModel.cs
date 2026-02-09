using AacV1.Core;
using AacV1.Services;

namespace AacV1.VM;

public class PcControlViewModel : ObservableObject
{
    private readonly MainViewModel _mainViewModel;
    private readonly PcControlService _pcControlService;

    public PcControlViewModel(MainViewModel mainViewModel, PcControlService pcControlService)
    {
        _mainViewModel = mainViewModel;
        _pcControlService = pcControlService;
        MoveUpCommand = new RelayCommand(_ => _pcControlService.MoveCursor(0, -20));
        MoveDownCommand = new RelayCommand(_ => _pcControlService.MoveCursor(0, 20));
        MoveLeftCommand = new RelayCommand(_ => _pcControlService.MoveCursor(-20, 0));
        MoveRightCommand = new RelayCommand(_ => _pcControlService.MoveCursor(20, 0));
        LeftClickCommand = new RelayCommand(_ => _pcControlService.LeftClick());
        RightClickCommand = new RelayCommand(_ => _pcControlService.RightClick());
        DoubleClickCommand = new RelayCommand(_ => _pcControlService.DoubleClick());
        DragStartCommand = new RelayCommand(_ => _pcControlService.DragStart());
        DragEndCommand = new RelayCommand(_ => _pcControlService.DragEnd());
        ScrollUpCommand = new RelayCommand(_ => _pcControlService.Scroll(120));
        ScrollDownCommand = new RelayCommand(_ => _pcControlService.Scroll(-120));
        AltTabCommand = new RelayCommand(_ => _pcControlService.AltTab());
        WinDCommand = new RelayCommand(_ => _pcControlService.WinD());
        CtrlLCommand = new RelayCommand(_ => _pcControlService.CtrlL());
    }

    public RelayCommand MoveUpCommand { get; }
    public RelayCommand MoveDownCommand { get; }
    public RelayCommand MoveLeftCommand { get; }
    public RelayCommand MoveRightCommand { get; }
    public RelayCommand LeftClickCommand { get; }
    public RelayCommand RightClickCommand { get; }
    public RelayCommand DoubleClickCommand { get; }
    public RelayCommand DragStartCommand { get; }
    public RelayCommand DragEndCommand { get; }
    public RelayCommand ScrollUpCommand { get; }
    public RelayCommand ScrollDownCommand { get; }
    public RelayCommand AltTabCommand { get; }
    public RelayCommand WinDCommand { get; }
    public RelayCommand CtrlLCommand { get; }
}
