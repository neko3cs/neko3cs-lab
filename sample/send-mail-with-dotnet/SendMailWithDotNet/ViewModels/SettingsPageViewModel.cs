using SendMailWithDotNet;
using SendMailWithDotNet.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public partial class SettingsPageViewModel : ObservableObject
{
  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(Name))]
  private string _name;
  public string Name => _name;
  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(Address))]
  private string _address;
  public string Address => _address;
  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(Password))]
  private string _password;
  public string Password => _password;

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