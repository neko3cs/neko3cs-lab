using AspNetCoreBlazor.Core.Services;
using AspNetCoreBlazor.Core.Types;
using AspNetCoreBlazor.Maui.Services;
using AspNetCoreBlazor.Maui.Views;
using Microsoft.Extensions.Logging;

namespace AspNetCoreBlazor.Maui;

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
            services.AddSingleton(_ => new PlatformDetection(PlatformKind.Maui));
			services.AddSingleton<ISecureStorageService, SecureStorageService>();
#if DEBUG
			services.AddBlazorWebViewDeveloperTools();
#endif
		})
#if DEBUG
		.ConfigureLogging(builder =>
		{
			builder.Logging.AddDebug();
		})
#endif
		.Build();

	#region HelperMethod
	private static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder, Action<IServiceCollection> action)
	{
		action(builder.Services);
		return builder;
	}

	private static MauiAppBuilder ConfigureLogging(this MauiAppBuilder builder, Action<MauiAppBuilder> action)
	{
		action(builder);
		return builder;
	}
	#endregion
}
