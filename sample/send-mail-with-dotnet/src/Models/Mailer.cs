using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SendMailWithDotnet.Models;

public class Mailer(Account account)
{
  private const string SmtpHost = "smtp.gmail.com";
  private const int SmtpPort = 465;
  private readonly Account _account = account;

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
    toList.ForEach(message.To.Add);
    ccList?.ForEach(message.Cc.Add);
    bccList?.ForEach(message.Bcc.Add);
    message.Subject = subject;
    message.Body = new TextPart("plain") { Text = body };

#if DEBUG
    using var client = new SmtpClient(new ProtocolLogger(Console.OpenStandardOutput()));
#else
    using var client = new SmtpClient();
#endif
    await client.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.Auto);
    await client.AuthenticateAsync(_account.Address, _account.Password);
    await client.SendAsync(message);
    await client.DisconnectAsync(quit: true);
  }
}