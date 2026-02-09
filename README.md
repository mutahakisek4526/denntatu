# AacV1

WPF (.NET 8) / C# 12 / MVVM で構成した意思伝達支援アプリです。日本語UI固定、UserControl + ContentControl + DataTemplate のみで画面構成しています。

## 起動方法

1. Windows 11 以降で .NET 8 SDK をインストールします。
2. 本リポジトリで以下を実行します。

```bash
dotnet restore

dotnet build

dotnet run --project AacV1/AacV1.csproj
```

## キー操作

- Enter: 決定
- Esc: 戻る
- F1: 支援者画面
- F2: ホーム画面
- F3: セルフテスト
- F5: 走査開始/停止
- F6: 読み上げ
- F7: 停止
- F8: 視線(ドウェル)ON/OFF
- F9: 2スイッチモード切替

## 入力モード切替

設定画面で「入力方式」を変更します。

- キーボード: 矢印＋Enter
- 自動走査: 一定間隔でフォーカス移動
- 2スイッチ: Space=Next, Enter=Select（設定で変更）
- ドウェル: 2段階ドウェル（1段階=ハイライト、2段階=決定）

## セルフテスト

セルフテスト画面の「セルフテスト実行」を押すと以下を確認します。

1. JSON保存/読込
2. TTS Speak/Stop
3. 走査タイマー
4. ドウェル2段階
5. HTTP送信（localhost簡易サーバ）
6. WebView2環境作成
7. PC操作（テストターゲットをクリック）

画面下部の「手動テスト」も合わせて確認してください。

## バックアップ/復元

支援者画面の「バックアップ」を押すと %AppData%/AacV1/backup.zip に保存します。
「復元」を押すと同じ場所の zip を読み込みます。

## 保存ファイル

`%AppData%/AacV1/` に JSON とログを保存します。

- settings.json
- phrases.json
- history.json
- favorites.json
- dictionary.json
- env.json
- logs/app.log
