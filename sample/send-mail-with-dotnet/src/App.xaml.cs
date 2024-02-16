using SendMailWithDotnet.Models;

namespace SendMailWithDotnet;

public partial class App : Application
{
	public static Account Account;

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
