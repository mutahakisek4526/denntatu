namespace AacV1.Core.Models;

public enum InputMode
{
    Keyboard,
    AutoScan,
    TwoSwitch,
    Dwell,
}

public class Settings
{
    public InputMode InputMode { get; set; } = InputMode.Keyboard;
    public int ScanIntervalMs { get; set; } = 800;
    public int DwellFirstStageMs { get; set; } = 600;
    public int DwellSecondStageMs { get; set; } = 800;
    public double KeySize { get; set; } = 60;
    public double FontSize { get; set; } = 20;
    public string ThemeColor { get; set; } = "#2B579A";
    public int SpeechRate { get; set; } = 0;
    public int SpeechVolume { get; set; } = 100;
    public string SpeechVoice { get; set; } = string.Empty;
    public string TwoSwitchNextKey { get; set; } = "Space";
    public string TwoSwitchSelectKey { get; set; } = "Enter";

    public SmtpSettings Smtp { get; set; } = new();
}

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public bool UseSsl { get; set; } = true;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
}
