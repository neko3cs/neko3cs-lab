using System.Windows.Input;
using SendMailWithDotnet.Models;
using SendMailWithDotnet.Service;

namespace SendMailWithDotnet.ViewModels;

public partial class SettingsPageViewModel : ViewModelBase
{
  private string _name = string.Empty;
  private string _address = string.Empty;
  private string _password = string.Empty;
  private readonly IDialogService _dialogService;

  public string Name
  {
    get => _name;
    set => SetProperty(ref _name, value);
  }
  public string Address
  {
    get => _address;
    set => SetProperty(ref _address, value);
  }
  public string Password
  {
    get => _password;
    set => SetProperty(ref _password, value);
  }
  public ICommand SaveUserInfoCommand { get; private set; }

  public SettingsPageViewModel(IDialogService dialogService)
  {
    _dialogService = dialogService;
    SaveUserInfoCommand = new Command(async () => await SaveUserInfoAsync());
  }

  private async Task SaveUserInfoAsync()
  {
    try
    {
      if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Password))
      {
        await _dialogService.DisplayAlertAsync("空欄があります。", "すべての欄を入力してください。", "OK");
        return;
      }

      App.Account = new Account(Name, Address, Password);
      await _dialogService.DisplayAlertAsync("設定完了", "設定しました。", "OK");
    }
    catch (Exception ex)
    {
      await _dialogService.DisplayAlertAsync("設定エラー", ex.Message, "OK");
    }
  }
}