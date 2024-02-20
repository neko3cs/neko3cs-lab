using System.Windows.Input;
using MimeKit;
using SendMailWithDotnet.Models;
using SendMailWithDotnet.Service;

namespace SendMailWithDotnet.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{
  private string _to = string.Empty;
  private string _cc = string.Empty;
  private string _bcc = string.Empty;
  private string _subject = string.Empty;
  private string _body = string.Empty;
  private readonly IDialogService _dialogService;

  public string To
  {
    get => _to;
    set => SetProperty(ref _to, value);
  }
  public string Cc
  {
    get => _cc;
    set => SetProperty(ref _cc, value);
  }
  public string Bcc
  {
    get => _bcc;
    set => SetProperty(ref _bcc, value);
  }
  public string Body
  {
    get => _body;
    set => SetProperty(ref _body, value);
  }
  public string Subject
  {
    get => _subject;
    set => SetProperty(ref _subject, value);
  }
  public ICommand SendMailCommand { get; private set; }

  public MainPageViewModel(IDialogService dialogService)
  {
    _dialogService = dialogService;
    SendMailCommand = new Command(async () => await SendMail());
  }

  public async Task SendMail()
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
      var mailer = new Mailer(App.Account);
      await mailer.SendAsync(Subject, Body, toList, ccList, bccList);
    }
    catch (Exception ex)
    {
      await _dialogService.DisplayAlertAsync("メール送信エラー", ex.Message, "OK");
    }
  }
}