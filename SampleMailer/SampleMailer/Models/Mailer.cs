using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SampleMailer.Models
{
    public class Mailer
    {
        private readonly string SmtpHost = "smtp.gmail.com";
        private readonly int SmtpPort = 465;
        private readonly Account FromAccount;

        public Mailer(Account account)
        {
            FromAccount = account;
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
            message.From.Add(new MailboxAddress(FromAccount.Name, FromAccount.Address));

            toList.ForEach(to => message.To.Add(to));
            ccList?.ForEach(cc => message.Cc.Add(cc));
            bccList?.ForEach(bcc => message.Bcc.Add(bcc));

            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };


            try
            {
                using (var client = new SmtpClient())
                {
                    // サーバ接続
                    await client.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.SslOnConnect);
                    // SMTP認証
                    await client.AuthenticateAsync(FromAccount.Address, FromAccount.Password);
                    // メール送信
                    await client.SendAsync(message);
                    // サーバ接続解除
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}