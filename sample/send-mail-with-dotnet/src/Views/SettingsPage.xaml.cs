using SendMailWithDotnet.ViewModels;

namespace SendMailWithDotnet.Views;

public partial class SettingsPage : ContentPage
{
  public SettingsPage(SettingsPageViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }
}