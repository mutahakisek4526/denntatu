using System.Net;
using System.Net.Mail;
using AacV1.Core.Models;

namespace AacV1.Services;

public class MailService
{
    private Settings _settings;

    public MailService(Settings settings)
    {
        _settings = settings;
    }

    public void UpdateSettings(Settings settings) => _settings = settings;

    public async Task SendAsync(string to, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(_settings.Smtp.Host))
        {
            throw new InvalidOperationException("SMTP設定が未入力です。");
        }

        using var client = new SmtpClient(_settings.Smtp.Host, _settings.Smtp.Port)
        {
            EnableSsl = _settings.Smtp.UseSsl,
            Credentials = new NetworkCredential(_settings.Smtp.Username, _settings.Smtp.Password)
        };

        var from = string.IsNullOrWhiteSpace(_settings.Smtp.FromAddress)
            ? _settings.Smtp.Username
            : _settings.Smtp.FromAddress;

        using var message = new MailMessage(from, to, subject, body);
        await client.SendMailAsync(message).ConfigureAwait(false);
    }
}
