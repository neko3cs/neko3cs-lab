# README

Visual Basicで書いたコンソールアプリをVisual Studioなしでビルドするサンプルコード。

`build.ps1` を実行すると `.\publish\` に実行ファイル `a.exe` が生成される。

## 気づきメモ

- VS使わない場合Nugetパッケージはブラウザから取得する必要がある
  - `*.nupkg` 形式で取得されるため、拡張子を `*.zip` に変換して解凍する
  - 今回は以下から取得した
    - 🔗[NuGet Gallery | Newtonsoft.Json 13.0.2](https://www.nuget.org/packages/Newtonsoft.Json/)
- ビルドした実行ファイルを実行する際は参照したdllも手動でコピーして実行ファイルと同じパスに置く必要がある
