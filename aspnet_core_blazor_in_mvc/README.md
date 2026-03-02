# ASP.NET Core MVC + Blazor 都道府県・市区町村選択サンプル

このプロジェクトは、ASP.NET Core MVCのView (.cshtml) の中でBlazor Serverコンポーネント (.razor) を動作させ、JavaScriptを一切記述せずに動的な連動ドロップダウンを実現するサンプルアプリケーションです。

## 構成のポイント

- **Program.cs**: `AddServerSideBlazor()` および `MapBlazorHub()` を追加し、MVC内でBlazorを動作させるためのSignalR通信環境を設定しています。また、EF Core InMemoryを使用してデータを管理しています。
- **Components/LocationSelector.razor**: 都道府県の選択に応じて市区町村リストを非同期に更新するコンポーネントです。C# のロジック（サーバーサイド実行）のみでUIの差分更新を行います。
- **Index.cshtml**: MVCのビューの中で `<component>` タグヘルパーを使用して、Blazorコンポーネントを埋め込んでいます。`ServerPrerendered` モードにより初回表示時のSSRを実現しています。
- **wwwroot/data/locations.json**: アプリケーション起動時にデータベースへシードされる都道府県・市区町村のマスターデータです。

## 動作の仕組み

1. **初回表示 (SSR)**: ページアクセス時、サーバー側でコンポーネントがレンダリングされ、静的なHTMLとしてブラウザに送られます。
2. **インタラクション (SignalR)**: ユーザーが都道府県を選択すると、SignalRを通じてサーバー側のC# メソッド（`OnPrefectureChange`）が実行されます。
3. **差分更新**: サーバー側で更新されたデータに基づき、Blazorが変更が必要なDOM要素のみを特定し、最小限の差分データのみをブラウザに送ってUIを更新します。

## 使い方

1. プロジェクトをビルドして実行します。
   ```bash
   dotnet run --project AspNetCoreBlazorInMvc/AspNetCoreBlazorInMvc.csproj
   ```
2. ブラウザで `https://localhost:5001` 等にアクセスします。
3. 都道府県を選択すると、即座に市区町村ドロップダウンが表示され、選択した場所がラベルに反映されます。

## 利点

- **JavaScript ゼロ**: イベントハンドリングからデータ取得まで、すべて型安全なC# で記述できます。
- **高効率な通信**: AjaxによるHTML全体の差し替えではなく、SignalRによるバイナリ形式の差分更新のため、非常に高速で滑らかな操作感を提供します。
- **既存資産の活用**: ページ全体をBlazorに移行することなく、複雑なフォームや動的パーツのみをBlazor化することが可能です。
