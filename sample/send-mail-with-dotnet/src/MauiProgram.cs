using Microsoft.Extensions.Logging;
using SendMailWithDotnet.Views;
using SendMailWithDotnet.ViewModels;
using SendMailWithDotnet.Service;

namespace SendMailWithDotnet;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp() => MauiApp.CreateBuilder()
		.UseMauiApp<App>()
		.RegisterDependencies()
		.ConfigureLogging()
		.ConfigureFonts(fonts =>
		{
			fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
		})
		.Build();

	public static MauiAppBuilder ConfigureLogging(this MauiAppBuilder mauiAppBuilder)
	{
#if DEBUG
		mauiAppBuilder.Logging.AddDebug();
#endif

		return mauiAppBuilder;
	}

	public static MauiAppBuilder RegisterDependencies(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services
			.AddTransient<MainPage>()
			.AddTransient<MainPageViewModel>()
			.AddTransient<SettingsPage>()
			.AddTransient<SettingsPageViewModel>()
			.AddTransient<IDialogService, DialogService>();

		return mauiAppBuilder;
	}
}