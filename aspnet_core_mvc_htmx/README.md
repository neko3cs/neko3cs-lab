# ASP.NET Core MVC + HTMX PartialView 連動サンプル

このプロジェクトは、ASP.NET Core MVCとHTMXを組み合わせ、JavaScriptを一切書かずに動的なUI（連動ドロップダウン）を実現したサンプルアプリケーションです。

## プロジェクト概要
日本全国の都道府県（47）と市区町村（約1,700）のデータを使用し、都道府県を選択すると該当する市区町村が動的に読み込まれ、選択された場所がラベルとして表示される機能を実装しています。

## このサンプルコードで分かったこと
- **JavaScript ゼロの実現**: HTMXを使用することで、従来 `fetch` や `axios` とイベントリスナーで記述していた動的なUI更新を、すべてHTML属性とサーバーサイドのPartialView制御で完結させることができました。
- **MPA の快適な操作性**: ページ全体をリロードすることなく、必要な箇所だけをPartialViewとして差し替えることで、MPA (Multi-Page Application) の堅牢さとSPA (Single-Page Application) のような滑らかな操作性を両立できます。
- **サーバーサイド主導の UI**: 表示ロジック（ラベルの生成など）をサーバー側に集約できるため、クライアントサイドのコードを簡略化し、保守性を高めることができます。

## HTMX による PartialView 埋め込みの解説

### 1. `hx-get` と `hx-target` による動的取得
都道府県ドロップダウンが変更されると、HTMXが `hx-get` で指定されたエンドポイントを叩き、返却されたHTML（PartialView）を `hx-target` で指定された要素の中に差し込みます。

```html
<select hx-get="/Home/Cities" hx-target="#city-container" hx-trigger="change">
    ...
</select>
```

### 2. `hx-swap-oob` による複数箇所の同時更新
都道府県が変わった際、市区町村リストを更新すると同時に「選択中の場所」ラベルを「未選択」にリセットする必要があります。HTMXの **Out-of-Band (OOB) Swap** 機能を使うと、レスポンスの中に含まれる特定の要素を、本来のターゲット（`#city-container`）の外にある要素（`#display-selection`）に自動的に反映させることができます。

```html
<!-- _CitiesPartial.cshtml 内 -->
<span id="display-selection" hx-swap-oob="true">未選択</span>
<select id="city"> ... </select>
```

## 実行方法

1. **リポジトリをクローンまたはダウンロード**
2. **プロジェクトディレクトリへ移動**
   ```bash
   cd AspNetCoreMvcHtmx
   ```
3. **アプリケーションの実行**
   ```bash
   dotnet run
   ```
4. **ブラウザでアクセス**
   デフォルトでは `http://localhost:5133` または `https://localhost:7133` で起動します。

## 技術スタック
- **ASP.NET Core MVC 10.0**
- **HTMX (CDN)**: `https://unpkg.com/htmx.org`
- **Entity Framework Core**: InMemory Databaseを使用。起動時に `locations.json` からデータをシードします。
- **Bootstrap 5**: クリーンでモダンなUIスタイリング。

## データソース
総務省が公開している「全国地方公共団体コード」（令和6年11月11日時点）のExcelデータを加工して使用しています。
