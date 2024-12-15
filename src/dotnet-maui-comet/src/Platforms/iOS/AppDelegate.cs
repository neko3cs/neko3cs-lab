using Foundation;
using Microsoft.Maui.Hosting;

namespace BasicDotnetMauiComet;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => App.CreateMauiApp();
}

