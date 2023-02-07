using Microsoft.AspNetCore.Components.WebView.Maui;
using PracticeDotNetMauiBlazor.Data;

namespace PracticeDotNetMauiBlazor;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp() => MauiApp.CreateBuilder()
        .UseMauiApp<App>()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        })
        .ConfigureServices(services =>
        {
            services.AddMauiBlazorWebView();
#if DEBUG
            services.AddBlazorWebViewDeveloperTools();
#endif
            services.AddSingleton<WeatherForecastService>();
        })
        .Build();
}

#region MauiAppBuilderExtensions
public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder, Action<IServiceCollection> action)
    {
        action(builder.Services);
        return builder;
    }
}
#endregion