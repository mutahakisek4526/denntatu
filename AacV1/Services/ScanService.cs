using System.Windows.Threading;

namespace AacV1.Services;

public class ScanService
{
    private readonly DispatcherTimer _timer;
    private readonly List<Action> _targets = new();
    private int _index;

    public ScanService(int intervalMs)
    {
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(intervalMs)
        };
        _timer.Tick += (_, _) => MoveNext();
    }

    public bool IsRunning => _timer.IsEnabled;

    public event Action<int>? IndexChanged;

    public void UpdateInterval(int intervalMs)
    {
        _timer.Interval = TimeSpan.FromMilliseconds(intervalMs);
    }

    public void SetTargets(IEnumerable<Action> targets)
    {
        _targets.Clear();
        _targets.AddRange(targets);
        _index = 0;
        IndexChanged?.Invoke(_index);
    }

    public void Start()
    {
        if (_targets.Count == 0)
        {
            return;
        }
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void MoveNext()
    {
        if (_targets.Count == 0)
        {
            return;
        }
        _index = (_index + 1) % _targets.Count;
        IndexChanged?.Invoke(_index);
    }

    public void Select()
    {
        if (_targets.Count == 0)
        {
            return;
        }
        _targets[_index]?.Invoke();
    }
}
