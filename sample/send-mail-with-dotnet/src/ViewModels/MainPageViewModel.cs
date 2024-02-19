using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MimeKit;
using SendMailWithDotnet.Models;
using SendMailWithDotnet.Service;

namespace SendMailWithDotnet.ViewModels;

public partial class MainPageViewModel : INotifyPropertyChanged
{
  private string _to = string.Empty;
  private string _cc = string.Empty;
  private string _bcc = string.Empty;
  private string _subject = string.Empty;
  private string _body = string.Empty;
  public event PropertyChangedEventHandler PropertyChanged;
  private readonly IDialogService _dialogService;

  public string To
  {
    get => _to;
    set
    {
      if (_to != value)
      {
        _to = value;
        OnPropertyChanged();
      }
    }
  }
  public string Cc
  {
    get => _cc;
    set
    {
      if (_cc != value)
      {
        _cc = value;
        OnPropertyChanged();
      }
    }
  }
  public string Bcc
  {
    get => _bcc;
    set
    {
      if (_bcc != value)
      {
        _bcc = value;
        OnPropertyChanged();
      }
    }
  }
  public string Body
  {
    get => _body;
    set
    {
      if (_body != value)
      {
        _body = value;
        OnPropertyChanged();
      }
    }
  }
  public string Subject
  {
    get => _subject;
    set
    {
      if (_subject != value)
      {
        _subject = value;
        OnPropertyChanged();
      }
    }
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
      var mailer = new SendMail(App.Account);
      await mailer.SendAsync(Subject, Body, toList, ccList, bccList);
    }
    catch (Exception ex)
    {
      await _dialogService.DisplayAlertAsync("メール送信エラー", ex.Message, "OK");
    }
  }

  public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}