using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MimeKit;
using SendMailWithDotnet.Models;

namespace SendMailWithDotnet.ViewModels;

internal partial class MainPageViewModel : ObservableObject
{
  [ObservableProperty]
  private string to;

  [ObservableProperty]
  private string cc;

  [ObservableProperty]
  private string bcc;

  [ObservableProperty]
  private string subject;

  [ObservableProperty]
  private string body;

  [RelayCommand]
  private async Task SendMailCommand()
  {
    var toList = To.Split([','])
        .Select(address => new MailboxAddress(address.Trim(), address.Trim()))
        .ToList();
    var ccList = Cc.Split([','])
        .Select(address => new MailboxAddress(address.Trim(), address.Trim()))
        .ToList();
    var bccList = Bcc.Split([','])
        .Select(address => new MailboxAddress(address.Trim(), address.Trim()))
        .ToList();

    try
    {
      var mailer = new SendMail(App.Account);
      await mailer.SendAsync(Subject, Body, toList, ccList, bccList);
    }
    catch (Exception)
    {
      //await DisplayAlertAsync("Send Email Error", ex.Message, "OK");
    }
  }
}