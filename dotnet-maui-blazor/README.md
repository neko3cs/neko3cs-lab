# Practice .NET MAUI BLAZOR

.NET MAUI+Blazorでアプリ作ってみたコード。

## 技術メモ

### MauiのSecure Storage

パスワードとかローカルに安全に保存できる。

- [セキュリティで保護されたストレージ - .NET MAUI | Microsoft Learn](https://learn.microsoft.com/ja-jp/dotnet/maui/platform-integration/storage/secure-storage?view=net-maui-8.0&tabs=macios)

### Razor Class Libによるコンポーネントのプロジェクト分離

以下のコマンドでプロジェクトを生成する。

```zsh
dotnet new razorclasslib -o <ComponentFullName>
```

呼び出し元の_Imports.razorにRCLの名前空間を `@using` しておく。

Main.razorにアセンブリを管理する配列のフィールドを定義してRouterコンポーネントに `AdditionalAssemblies` 属性を追加する。

```cs
<Router AppAssembly="@typeof(Main).Assembly" AdditionalAssemblies="@LazyLoadAssemblies">
  <Found Context="routeData">

// 中略

@code {
  Assembly[] LazyLoadAssemblies = new[] { typeof(MauiBlazorApp.RCL._Imports).Assembly };
}
```

あとはNavMenu.razorなどで他のコンポーネントと同様にNavLinkする。

```cs
<NavLink class="nav-link" href="helloworld">
```

- [ASP.NET Core Razor コンポーネントを Razor クラス ライブラリ (RCL) から使用する | Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/blazor/components/class-libraries?view=aspnetcore-8.0&tabs=visual-studio-code)
