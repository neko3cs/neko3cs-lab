using AspNetCoreBlazor.Core.Services;
using AspNetCoreBlazor.Core.Types;
using AspNetCoreBlazor.Wpf.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Windows;

namespace AspNetCoreBlazor.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var services = new ServiceCollection();
        services.AddWpfBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        // TODO: HttpClientでwwwroot内のリソースにアクセスする方法があるのか調べる
        services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("http://localhost") });
        services.AddSingleton(_ => new PlatformDetection(PlatformKind.Wpf));
        services.AddSingleton<ISecureStorageService, SecureStorageService>();
        Resources.Add("services", services.BuildServiceProvider());
    }
}
