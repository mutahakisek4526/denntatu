using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using AacV1.Core;
using AacV1.Services;
using Microsoft.Web.WebView2.Core;
using System.Windows;

namespace AacV1.VM;

public class SelfTestViewModel : ObservableObject, IViewLifecycle
{
    private readonly MainViewModel _mainViewModel;
    private readonly DataStore _dataStore;
    private readonly SpeechService _speechService;
    private readonly ScanService _scanService;
    private readonly DwellService _dwellService;
    private readonly EnvControlService _envControlService;
    private readonly PcControlService _pcControlService;

    private string _manualSteps = "手動テスト: \n1) PC操作パネルでマウスが動くか確認\n2) Web画面でWebView2が表示されURL移動できるか確認\n3) メール画面でSMTP送信を実施\n4) 環境制御でHTTP送信を実施";

    private bool _pcTestClicked;
    private Point _pcTargetPosition;

    public SelfTestViewModel(
        MainViewModel mainViewModel,
        DataStore dataStore,
        SpeechService speechService,
        ScanService scanService,
        DwellService dwellService,
        EnvControlService envControlService,
        PcControlService pcControlService)
    {
        _mainViewModel = mainViewModel;
        _dataStore = dataStore;
        _speechService = speechService;
        _scanService = scanService;
        _dwellService = dwellService;
        _envControlService = envControlService;
        _pcControlService = pcControlService;
        Results = new ObservableCollection<SelfTestItem>();
        RunCommand = new AsyncRelayCommand(RunAllAsync);
    }

    public ObservableCollection<SelfTestItem> Results { get; }

    public string ManualSteps
    {
        get => _manualSteps;
        set => SetProperty(ref _manualSteps, value);
    }

    public AsyncRelayCommand RunCommand { get; }

    public void OnEnter() { }
    public void OnExit() { }

    public void SetPcTargetPosition(Point point)
    {
        _pcTargetPosition = point;
    }

    public void MarkPcTestClicked()
    {
        _pcTestClicked = true;
    }

    private async Task RunAllAsync()
    {
        Results.Clear();
        await TestJsonAsync();
        await TestTtsAsync();
        await TestScanAsync();
        await TestDwellAsync();
        await TestHttpAsync();
        await TestWebViewAsync();
        await TestPcControlAsync();
    }

    private async Task TestJsonAsync()
    {
        try
        {
            var settings = await _dataStore.LoadSettingsAsync();
            settings.ScanIntervalMs += 1;
            await _dataStore.SaveSettingsAsync(settings);
            var loaded = await _dataStore.LoadSettingsAsync();
            AddResult("JSON保存/読込", loaded.ScanIntervalMs == settings.ScanIntervalMs, "設定を読み書きしました");
        }
        catch (Exception ex)
        {
            AddResult("JSON保存/読込", false, ex.Message);
        }
    }

    private async Task TestTtsAsync()
    {
        try
        {
            await _speechService.SpeakAsync("セルフテスト");
            _speechService.Stop();
            AddResult("TTS Speak/Stop", true, "再生と停止を実行");
        }
        catch (Exception ex)
        {
            AddResult("TTS Speak/Stop", false, ex.Message);
        }
    }

    private Task TestScanAsync()
    {
        try
        {
            _scanService.SetTargets(new List<Action> { () => { } });
            _scanService.Start();
            _scanService.Stop();
            AddResult("走査タイマー", true, "開始と停止を実行");
        }
        catch (Exception ex)
        {
            AddResult("走査タイマー", false, ex.Message);
        }
        return Task.CompletedTask;
    }

    private async Task TestDwellAsync()
    {
        var tcs = new TaskCompletionSource<bool>();
        void StageTwo(int id) => tcs.TrySetResult(true);
        _dwellService.StageTwo += StageTwo;
        try
        {
            _dwellService.HoverEnter(1);
            var completed = await Task.WhenAny(tcs.Task, Task.Delay(1500));
            var success = completed == tcs.Task;
            AddResult("ドウェル2段階", success, success ? "2段階完了" : "時間内に完了せず");
        }
        catch (Exception ex)
        {
            AddResult("ドウェル2段階", false, ex.Message);
        }
        finally
        {
            _dwellService.StageTwo -= StageTwo;
            _dwellService.Reset();
        }
    }

    private async Task TestHttpAsync()
    {
        var listener = new HttpListener();
        var prefix = "http://localhost:18080/";
        listener.Prefixes.Add(prefix);
        try
        {
            listener.Start();
            var listenTask = listener.GetContextAsync();
            var device = new Core.Models.EnvDevice
            {
                Name = "SelfTest",
                Url = prefix,
                Method = "GET"
            };
            var responseText = await _envControlService.SendAsync(device);
            var context = await listenTask;
            var buffer = Encoding.UTF8.GetBytes("OK");
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer);
            context.Response.OutputStream.Close();
            AddResult("HTTP送信", responseText.Contains("200"), responseText);
        }
        catch (Exception ex)
        {
            AddResult("HTTP送信", false, ex.Message);
        }
        finally
        {
            listener.Stop();
        }
    }

    private async Task TestWebViewAsync()
    {
        try
        {
            await CoreWebView2Environment.CreateAsync();
            AddResult("WebView2表示", true, "WebView2環境作成成功");
        }
        catch (Exception ex)
        {
            AddResult("WebView2表示", false, ex.Message);
        }
    }

    private Task TestPcControlAsync()
    {
        try
        {
            _pcTestClicked = false;
            _pcControlService.MoveCursorTo(_pcTargetPosition.X, _pcTargetPosition.Y);
            _pcControlService.LeftClick();
            AddResult("PC操作", _pcTestClicked, _pcTestClicked ? "クリック成功" : "クリック未検出");
        }
        catch (Exception ex)
        {
            AddResult("PC操作", false, ex.Message);
        }
        return Task.CompletedTask;
    }

    private void AddResult(string name, bool passed, string details)
    {
        Results.Add(new SelfTestItem(name, passed, details));
    }
}

public record SelfTestItem(string Name, bool Passed, string Details);
