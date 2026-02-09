using System.Collections.ObjectModel;
using AacV1.Core;
using AacV1.Services;

namespace AacV1.VM;

public class KanaBoardViewModel : ObservableObject, Services.IViewLifecycle
{
    private readonly MainViewModel _mainViewModel;

    public KanaBoardViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        Keys = new ObservableCollection<KanaKey>(BuildKeys());
        SelectPredictionCommand = new RelayCommand<string>(SelectPrediction);
    }

    public ObservableCollection<KanaKey> Keys { get; }
    public AppState AppState => _mainViewModel.AppState;
    public RelayCommand<string> SelectPredictionCommand { get; }

    public void OnEnter()
    {
        _mainViewModel.RefreshPredictions();
    }

    public void OnExit() { }

    private IEnumerable<KanaKey> BuildKeys()
    {
        var list = new List<KanaKey>();
        var rows = new[]
        {
            "あ い う え お",
            "か き く け こ",
            "さ し す せ そ",
            "た ち つ て と",
            "な に ぬ ね の",
            "は ひ ふ へ ほ",
            "ま み む め も",
            "や ゆ よ",
            "ら り る れ ろ",
            "わ を ん",
        };

        foreach (var row in rows)
        {
            foreach (var token in row.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                list.Add(new KanaKey(token, () => Append(token)));
            }
        }

        list.Add(new KanaKey("゛", () => Append("゛")));
        list.Add(new KanaKey("゜", () => Append("゜")));
        list.Add(new KanaKey("小文字", () => Append("ぁ")));
        list.Add(new KanaKey("、", () => Append("、")));
        list.Add(new KanaKey("。", () => Append("。")));
        list.Add(new KanaKey("空白", () => Append(" ")));
        list.Add(new KanaKey("削除", Backspace));
        list.Add(new KanaKey("全消去", Clear));
        list.Add(new KanaKey("確定", Confirm));
        list.Add(new KanaKey("読み上げ", Speak));
        list.Add(new KanaKey("停止", StopSpeak));
        return list;
    }

    private void Append(string value)
    {
        AppState.CurrentText += value;
        _mainViewModel.RefreshPredictions();
    }

    private void Backspace()
    {
        if (AppState.CurrentText.Length > 0)
        {
            AppState.CurrentText = AppState.CurrentText[..^1];
            _mainViewModel.RefreshPredictions();
        }
    }

    private void Clear()
    {
        AppState.CurrentText = string.Empty;
        _mainViewModel.RefreshPredictions();
    }

    private void Confirm()
    {
        if (string.IsNullOrWhiteSpace(AppState.CurrentText))
        {
            return;
        }
        _mainViewModel.AppState.History.Insert(0, new Core.Models.HistoryEntry { Text = AppState.CurrentText });
        _mainViewModel.LearnFromText(AppState.CurrentText);
        _mainViewModel.SaveHistory();
        _mainViewModel.RefreshPredictions();
    }

    private void Speak() => _ = _mainViewModel.SpeakCommand.Execute(null);
    private void StopSpeak() => _mainViewModel.StopSpeakCommand.Execute(null);

    private void SelectPrediction(string? candidate)
    {
        if (string.IsNullOrWhiteSpace(candidate))
        {
            return;
        }
        AppState.CurrentText = candidate;
    }
}

public record KanaKey(string Label, Action Action)
{
    public RelayCommand Command => new(_ => Action());
}
