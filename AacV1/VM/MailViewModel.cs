using AacV1.Core;
using AacV1.Services;

namespace AacV1.VM;

public class MailViewModel : ObservableObject
{
    private readonly MainViewModel _mainViewModel;
    private readonly MailService _mailService;
    private string _to = string.Empty;
    private string _subject = string.Empty;
    private string _body = string.Empty;
    private string _status = string.Empty;

    public MailViewModel(MainViewModel mainViewModel, MailService mailService)
    {
        _mainViewModel = mainViewModel;
        _mailService = mailService;
        SendCommand = new AsyncRelayCommand(SendAsync);
    }

    public string To
    {
        get => _to;
        set => SetProperty(ref _to, value);
    }

    public string Subject
    {
        get => _subject;
        set => SetProperty(ref _subject, value);
    }

    public string Body
    {
        get => _body;
        set => SetProperty(ref _body, value);
    }

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public AsyncRelayCommand SendCommand { get; }

    private async Task SendAsync()
    {
        try
        {
            await _mailService.SendAsync(To, Subject, Body);
            Status = "送信完了";
        }
        catch (Exception ex)
        {
            Status = $"送信失敗: {ex.Message}";
        }
    }
}
