using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public class MainPageViewModel : ObservableObject
{
  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(To))]
  private string _to;
  public string To => _to;
  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(Cc))]
  private string _cc;
  public string Cc => _cc;
  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(Bcc))]
  private string _bcc;
  public string Bcc => _bcc;
  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(Subject))]
  private string _subject;
  public string Subject => _subject;
  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(Body))]
  private string _body;
  public string Body => _body;

  [AsyncRelayCommand]
  public async Task SendMailCommand()
  {
    var toList = To.Split(new char[] { ',' })
      .Select(address => new MailboxAddress(address.Trim()))
      .ToList();
    var ccList = Cc.Split(new char[] { ',' })?
      .Select(address => new MailboxAddress(address.Trim()))
      .ToList();
    var bccList = Bcc.Split(new char[] { ',' })?
      .Select(address => new MailboxAddress(address.Trim()))
      .ToList();

    try
    {
      var mailer = new Mailer(App.Account);
      await mailer.SendAsync(Subject, Body, toList, ccList, bccList);
    }
    catch (Exception ex)
    {
      await PageDialogService.DisplayAlertAsync("Send Email Error", ex.Message, "OK");
      return;
    }
  }
}