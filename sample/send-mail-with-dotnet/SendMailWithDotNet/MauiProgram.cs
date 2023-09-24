using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SendMailWithDotNet.Views;

namespace SendMailWithDotNet;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.RegisterViewModels()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddTransient<MainPage>();
		mauiAppBuilder.Services.AddTransient<MainPageViewModel>();
		mauiAppBuilder.Services.AddTransient<SettingsPage>();
		mauiAppBuilder.Services.AddTransient<SettingsPageViewModel>();

		return mauiAppBuilder;
	}
}