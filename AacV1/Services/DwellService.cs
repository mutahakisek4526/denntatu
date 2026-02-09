using System.Windows.Threading;

namespace AacV1.Services;

public class DwellService
{
    private readonly DispatcherTimer _timer;
    private readonly DispatcherTimer _secondTimer;
    private int _currentId = -1;
    private bool _enabled = true;

    public DwellService(int firstStageMs, int secondStageMs)
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(firstStageMs) };
        _secondTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(secondStageMs) };
        _timer.Tick += (_, _) => StageOneReached();
        _secondTimer.Tick += (_, _) => StageTwoReached();
    }

    public bool Enabled => _enabled;

    public event Action<int>? StageOne;
    public event Action<int>? StageTwo;

    public void UpdateTimings(int firstStageMs, int secondStageMs)
    {
        _timer.Interval = TimeSpan.FromMilliseconds(firstStageMs);
        _secondTimer.Interval = TimeSpan.FromMilliseconds(secondStageMs);
    }

    public void SetEnabled(bool enabled)
    {
        _enabled = enabled;
        Reset();
    }

    public void HoverEnter(int id)
    {
        if (!_enabled)
        {
            return;
        }
        _currentId = id;
        _timer.Start();
    }

    public void HoverLeave(int id)
    {
        if (_currentId == id)
        {
            Reset();
        }
    }

    public void Reset()
    {
        _timer.Stop();
        _secondTimer.Stop();
        _currentId = -1;
    }

    private void StageOneReached()
    {
        _timer.Stop();
        if (_currentId >= 0)
        {
            StageOne?.Invoke(_currentId);
            _secondTimer.Start();
        }
    }

    private void StageTwoReached()
    {
        _secondTimer.Stop();
        if (_currentId >= 0)
        {
            StageTwo?.Invoke(_currentId);
        }
    }
}
