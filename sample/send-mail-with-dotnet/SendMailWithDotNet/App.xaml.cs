using SendMailWithDotNet.Models;

namespace SendMailWithDotNet;

public partial class App : Application
{
	public static Account Account;

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
