# ASP.NET Core MVC + Blazor 共存サンプル

このプロジェクトは、ASP.NET Core MVCのView (.cshtml) の中でBlazor Serverコンポーネント (.razor) を動作させるサンプルです。

## 構成のポイント

- **Program.cs**: `AddServerSideBlazor()` を追加し、`MapBlazorHub()` でSignalR通信用のエンドポイントを設定しています。
- **Components/UserList.razor**: C# のみでユーザーの追加や一覧表示を行うインタラクティブなコンポーネントです。
- **Index.cshtml**: MVCのビューの中で `<component>` タグヘルパーを使用して、Blazorコンポーネントを埋め込んでいます。
- **_Layout.cshtml**: `_framework/blazor.server.js` を読み込み、クライアントサイドでのBlazor実行環境を整えています。

## 使い方

1. プロジェクトをビルドして実行します。
2. ブラウザでアクセスすると、MVCのページの中に「ユーザー一覧 (Blazor)」というエリアが表示されます。
3. JavaScriptを使わずに、C# のロジックだけでユーザーの追加がリアルタイムに行えます。

## 利点

- JavaScriptを極力書かずに、C# で動的なUIを開発できます。
- 既存のMVCプロジェクトに対して、部分的にBlazorを導入できます。
- Microsoftのメインストリームな技術スタックであり、高い型安全性と保守性を持ちます。
