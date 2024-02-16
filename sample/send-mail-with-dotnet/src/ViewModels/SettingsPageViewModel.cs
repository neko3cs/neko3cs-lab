using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SendMailWithDotnet.Models;

namespace SendMailWithDotnet.ViewModels;

internal partial class SettingsPageViewModel : ObservableObject
{
  [ObservableProperty]
  public string name;

  [ObservableProperty]
  public string address;

  [ObservableProperty]
  public string password;

  [RelayCommand]
  public void SaveUserInfoCommand()
  {
    if (
        string.IsNullOrEmpty(Name) ||
        string.IsNullOrEmpty(Address) ||
        string.IsNullOrEmpty(Password)
    )
    {
      // await PageDialogService.DisplayAlertAsync("There is blank area.", "Please check input area.", "OK");
      return;
    }

    App.Account = new Account(
      Name,
      Address,
      Password
    );
  }
}