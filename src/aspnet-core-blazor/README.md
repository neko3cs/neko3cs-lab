# ASP.NET Core Blazor

ASP.NET Core Blazorのサンプルコードです。

## プロジェクト構成

プロジェクト構成は以下のようになっています。

|プロジェクト名|解説|
|--|--|
|AspNetCoreBlazor.Core|ここにBlazorコンポーネントを定義しています。プラットフォームに依存しないように `MainLayout.razor` から定義されているので各プラットフォームにて `Router` コンポーネントでCoreアセンブリをロードするだけで起動できるようにしています。|
|AspNetCoreBlazor.Maui|MAUIのプラットフォームプロジェクトです。|
