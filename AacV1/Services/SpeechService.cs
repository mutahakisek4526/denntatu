using System.Speech.Synthesis;
using AacV1.Core.Models;

namespace AacV1.Services;

public class SpeechService
{
    private readonly SpeechSynthesizer _synth = new();
    private readonly object _lock = new();

    public SpeechService(Settings settings)
    {
        ApplySettings(settings);
    }

    public void ApplySettings(Settings settings)
    {
        lock (_lock)
        {
            _synth.Rate = settings.SpeechRate;
            _synth.Volume = settings.SpeechVolume;
            if (!string.IsNullOrWhiteSpace(settings.SpeechVoice))
            {
                try
                {
                    _synth.SelectVoice(settings.SpeechVoice);
                }
                catch
                {
                }
            }
        }
    }

    public IEnumerable<string> GetVoices() => _synth.GetInstalledVoices().Select(v => v.VoiceInfo.Name);

    public Task SpeakAsync(string text)
    {
        return Task.Run(() =>
        {
            lock (_lock)
            {
                _synth.SpeakAsyncCancelAll();
                _synth.Speak(text);
            }
        });
    }

    public void Stop()
    {
        lock (_lock)
        {
            _synth.SpeakAsyncCancelAll();
        }
    }
}
