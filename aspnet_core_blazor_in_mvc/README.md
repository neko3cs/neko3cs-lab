# ASP.NET Core MVC + Blazor (Interactive Server) 都道府県・市区町村選択サンプル

このプロジェクトは、ASP.NET Core MVCのView (.cshtml) の中で **Blazor Interactive Server** コンポーネント (.razor) を動作させ、JavaScriptを一切記述せずに動的な連動ドロップダウンを実現するサンプルアプリケーションです。

.NET 8以降の推奨構成である **Razor Components** の仕組みを活用しています。

## 構成のポイント

- **Program.cs**: `AddRazorComponents().AddInteractiveServerComponents()` を使用してBlazorサービスを登録し、`MapRazorComponents<App>().AddInteractiveServerRenderMode()` でエンドポイントを設定しています。また、.NET 8以降で必須となる `app.UseAntiforgery()` を適切に配置しています。
- **Components/App.razor**: Blazor統合に必要最小限なルートコンポーネントです。ハイブリッド構成のため、HTMLルート（html, body等）はMVC側のLayoutに任せて空にしています。
- **Components/LocationSelector.razor**: `@rendermode InteractiveServer` を指定したインタラクティブなコンポーネントです。C# のロジックのみでUIの差分更新を行います。
- **Index.cshtml**: MVCのビューの中で `<component>` タグヘルパーを使用して、Blazorコンポーネントを埋め込んでいます。コンポーネント側で描画モードを管理しているため、呼び出し側は `Static` モードを使用しています（これにより、初回SSRとその後のインタラクティブ化が両立します）。

## 動作の仕組み

1. **初回表示 (SSR)**: ページアクセス時、サーバー側でコンポーネントがプリレンダリングされ、静的なHTMLとしてブラウザに送られます。
2. **有効化 (Hydration)**: ブラウザに読み込まれた `blazor.web.js` が、SSRされた要素を検出し、SignalR接続を確立してコンポーネントをインタラクティブにします。
3. **差分更新**: ユーザー操作が発生すると、SignalRを通じてサーバー側のC# メソッドが実行され、変更が必要なDOM要素の最小限の差分データのみがブラウザに送られてUIが更新されます。

## 使い方

1. プロジェクトを起動します（ホットリロード有効）。
   ```bash
   dotnet watch --project AspNetCoreBlazorInMvc/AspNetCoreBlazorInMvc.csproj
   ```
2. ブラウザで `http://localhost:5204` 等にアクセスします。
3. 都道府県を選択すると、即座に市区町村ドロップダウンが表示され、選択した場所がラベルに反映されます。

---

## 素の ASP.NET Core MVC プロジェクトへの導入手順 (.NET 8+)

既存のMVCプロジェクトにBlazor Interactive Server機能を実装する最小限の手順です。

### 1. プロジェクトの設定 (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Blazor サービスの追加
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery(); // 必須：CSRF 対策

// Blazor コンポーネントのマッピング
app.MapRazorComponents<YourProject.Components.App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultControllerRoute();
app.Run();
```

### 2. ルートコンポーネントの作成 (Components/App.razor)

`Components` フォルダーを作成し、以下の内容でファイルを作成します。

```razor
@* ハイブリッド構成では MVC 側が HTML 構造を提供するため、内容は空で構いません *@
```

### 3. レイアウトの修正 (Views/Shared/_Layout.cshtml)

Blazorの実行に必要なスクリプトを追加します。

```html
    ...
    <script src="_framework/blazor.web.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### 4. Blazor コンポーネントの作成 (例: Components/Counter.razor)

先頭に `@rendermode InteractiveServer` を記述するのがポイントです。

```razor
@rendermode InteractiveServer

<button class="btn btn-primary" @onclick="IncrementCount">Count: @currentCount</button>

@code {
    private int currentCount = 0;
    private void IncrementCount() => currentCount++;
}
```

### 5. MVC ビューへの埋め込み (Index.cshtml 等)

```html
<h3>MVC ページ内の Blazor</h3>
<component type="typeof(YourProject.Components.Counter)" render-mode="Static" />
```

*注意: 名前空間を簡略化するために `_ViewImports.cshtml` に `@using YourProject.Components` を追加することをお勧めします。*
