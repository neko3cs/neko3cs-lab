using Microsoft.Maui.Hosting;

namespace BasicDotnetMauiComet;

public class App : CometApp
{
    [Body]
    private View View() => new MainPage();

    public static MauiApp CreateMauiApp() => MauiApp
        .CreateBuilder()
        .UseCometApp<App>()
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        })
        //-:cnd
#if DEBUG
        .EnableHotReload()
#endif
        //+:cnd
        .Build();
}