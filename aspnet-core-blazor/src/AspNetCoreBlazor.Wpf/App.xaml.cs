using AspNetCoreBlazor.Core.Services;
using AspNetCoreBlazor.Core.Types;
using AspNetCoreBlazor.Wpf.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Windows;

namespace AspNetCoreBlazor.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        var services = new ServiceCollection();
        services.AddWpfBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        services.AddScoped(_ => new HttpClient());
        services.AddSingleton(_ => new PlatformDetection(PlatformKind.Wpf));
        services.AddSingleton<IUserService, UserService>();
        Resources.Add("services", services.BuildServiceProvider());
    }
}
