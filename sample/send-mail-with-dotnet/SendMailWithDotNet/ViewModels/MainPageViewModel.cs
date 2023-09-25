using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MimeKit;
using SendMailWithDotNet.Models;

namespace SendMailWithDotNet.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    [field: ObservableProperty]
    [field: NotifyPropertyChangedFor(nameof(To))]
    public string To { get; private set; }

    [field: ObservableProperty]
    [field: NotifyPropertyChangedFor(nameof(Cc))]
    public string Cc { get; private set; }

    [field: ObservableProperty]
    [field: NotifyPropertyChangedFor(nameof(Bcc))]
    public string Bcc { get; private set; }

    [field: ObservableProperty]
    [field: NotifyPropertyChangedFor(nameof(Subject))]
    public string Subject { get; private set; }

    [field: ObservableProperty]
    [field: NotifyPropertyChangedFor(nameof(Body))]
    public string Body { get; private set; }

    [RelayCommand]
    public async Task SendMailCommand()
    {
        var toList = To.Split(new char[] { ',' })
            .Select(address => new MailboxAddress(address.Trim(), address.Trim()))
            .ToList();
        var ccList = Cc.Split(new char[] { ',' })
            .Select(address => new MailboxAddress(address.Trim(), address.Trim()))
            .ToList();
        var bccList = Bcc.Split(new char[] { ',' })
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