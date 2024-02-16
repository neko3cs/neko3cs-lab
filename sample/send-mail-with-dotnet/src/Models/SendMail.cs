using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SendMailWithDotnet.Models;

public class SendMail
{
  private const string SmtpHost = "smtp.gmail.com";
  private const int SmtpPort = 465;
  private readonly Account _account;

  public SendMail(Account account)
  {
    _account = account;
  }

  public async Task SendAsync(
    string subject,
    string body,
    List<MailboxAddress> toList,
    List<MailboxAddress> ccList = null,
    List<MailboxAddress> bccList = null
  )
  {
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress(_account.Name, _account.Address));

    toList.ForEach(to => message.To.Add(to));
    ccList?.ForEach(cc => message.Cc.Add(cc));
    bccList?.ForEach(bcc => message.Bcc.Add(bcc));

    message.Subject = subject;
    message.Body = new TextPart("plain") { Text = body };

    using var client = new SmtpClient();
    // サーバ接続
    await client.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.SslOnConnect);
    // SMTP認証
    await client.AuthenticateAsync(_account.Address, _account.Password);
    // メール送信
    await client.SendAsync(message);
    // サーバ接続解除
    await client.DisconnectAsync(true);
  }
}