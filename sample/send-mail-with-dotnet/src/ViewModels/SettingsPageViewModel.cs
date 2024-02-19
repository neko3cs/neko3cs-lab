using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SendMailWithDotnet.Models;
using SendMailWithDotnet.Service;

namespace SendMailWithDotnet.ViewModels;

public partial class SettingsPageViewModel : INotifyPropertyChanged
{
  private string _name = string.Empty;
  private string _address = string.Empty;
  private string _password = string.Empty;
  public event PropertyChangedEventHandler PropertyChanged;
  private readonly IDialogService _dialogService;

  public string Name
  {
    get => _name;
    set
    {
      if (_name != value)
      {
        _name = value;
        OnPropertyChanged();
      }
    }
  }
  public string Address
  {
    get => _address;
    set
    {
      if (_address != value)
      {
        _address = value;
        OnPropertyChanged();
      }
    }
  }
  public string Password
  {
    get => _password;
    set
    {
      if (_password != value)
      {
        _password = value;
        OnPropertyChanged();
      }
    }
  }
  public ICommand SaveUserInfoCommand { get; private set; }

  public SettingsPageViewModel(IDialogService dialogService)
  {
    _dialogService = dialogService;
    SaveUserInfoCommand = new Command(async () => await SaveUserInfoAsync());
  }

  private async Task SaveUserInfoAsync()
  {
    if (
        string.IsNullOrEmpty(Name) ||
        string.IsNullOrEmpty(Address) ||
        string.IsNullOrEmpty(Password)
    )
    {
      await _dialogService.DisplayAlertAsync("空欄があります。", "すべての欄を入力してください。", "OK");
    }

    App.Account = new Account(
      Name,
      Address,
      Password
    );
  }

  public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}