# go_cobra_cli

[Cobra](https://github.com/spf13/cobra) を使ったGo製CLIツールのサンプルです。
サブコマンド・フラグ・位置引数の基本パターンを学ぶ目的で作成しました。

## 必要環境

- Go 1.21 以降

```bash
go version
```

## セットアップ

```bash
git clone <このリポジトリのURL>
cd go_cobra_cli
go mod tidy
```

`go mod tidy` は `go.mod` / `go.sum` に記載された依存関係（Cobraなど）を整合性チェックしつつダウンロードするコマンドです。過不足があれば自動で追加・削除してくれるので、`git clone` 直後や依存パッケージを追加した後は基本これを実行すればOKです。

## 使い方

### 実行（ビルドせずそのまま動かす）

```bash
go run . greet --name みなと --count 2
go run . weather Tokyo
go run . --help
```

### バイナリとしてビルドして使う

```bash
go build -o go_cobra_cli .
./go_cobra_cli greet -n みなと
./go_cobra_cli weather Tokyo
```

## コマンド一覧

### `greet`

指定した名前に挨拶を表示します。

```bash
go run . greet --name <名前> --count <回数>
```

| フラグ    | 短縮形 | 説明               | デフォルト |
| --------- | ------ | ------------------ | ---------- |
| `--name`  | `-n`   | 挨拶する相手の名前 | `world`    |
| `--count` | `-c`   | 繰り返す回数       | `1`        |

例:

```bash
go run . greet -n みなと -c 3
```

### `weather`

指定した都市の天気情報を表示します（サンプル固定値）。

```bash
go run . weather <city>
```

| 引数   | 説明                                  |
| ------ | ------------------------------------- |
| `city` | 天気を調べたい都市名（必須・1個のみ） |

例:

```bash
go run . weather Tokyo
```

## プロジェクト構成

```
go_cobra_cli/
├── main.go        # エントリーポイント。cmd.Execute() を呼ぶだけの薄い層
├── cmd/
│   ├── root.go     # ルートコマンド定義（rootCmd, Execute関数）
│   ├── greet.go    # greet サブコマンド
│   └── weather.go  # weather サブコマンド
├── go.mod
└── go.sum
```

各サブコマンドは同じ `cmd` パッケージ内で `init()` を使い、`rootCmd.AddCommand(...)` を呼んで自身を登録しています。`init()` はGoランタイムがパッケージ初期化時に自動実行するため、明示的な呼び出しコードは不要です（実行順はファイル名のアルファベット順）。

## サブコマンドを追加する方法

1. `cmd/新しい名前.go` を作成
2. `*cobra.Command` を定義し、`RunE` に処理を書く
3. `init()` の中で `rootCmd.AddCommand(...)` を呼ぶ

必要なフラグを増やしたい場合は `Flags()`（そのコマンド限定）または `PersistentFlags()`（子コマンドにも継承）を使います。

参考: 雛形生成を自動化したい場合は公式の `cobra-cli` も使えます。

```bash
go install github.com/spf13/cobra-cli@latest
cobra-cli add <コマンド名>
```

## 参考リンク

- Cobra 公式リポジトリ: https://github.com/spf13/cobra
- Cobra 公式チュートリアル: https://cobra.dev/docs/tutorials/first-cli/
