using System.Text;

namespace AacV1.Core;

public class Logging
{
    private readonly string _logPath;
    private readonly object _sync = new();

    public Logging(string logPath)
    {
        _logPath = logPath;
        Directory.CreateDirectory(Path.GetDirectoryName(_logPath)!);
    }

    public void Info(string message) => Write("INFO", message);
    public void Error(string message, Exception? ex = null)
    {
        var detail = ex is null ? message : $"{message} | {ex}";
        Write("ERROR", detail);
    }

    public string ReadAll() => File.Exists(_logPath) ? File.ReadAllText(_logPath, Encoding.UTF8) : string.Empty;

    private void Write(string level, string message)
    {
        var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}";
        lock (_sync)
        {
            File.AppendAllText(_logPath, line, Encoding.UTF8);
        }
    }
}
