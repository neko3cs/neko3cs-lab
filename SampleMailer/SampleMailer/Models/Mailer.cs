using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SampleMailer.Models
{
    public class Mailer
    {
        private readonly string SmtpHost = "smtp.gmail.com";

        private readonly int SmtpPort = 465;

        private UserAccount FromAccount;


        public Mailer(UserAccount account)
        {
            FromAccount = account;
        }

        public async Task SendAsync(List<MailboxAddress> toList, List<MailboxAddress> ccList, List<MailboxAddress> bccList, string subject, string body)
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
                    await client.ConnectAsync(SmtpHost, SmtpPort, SecureSocketOptions.SslOnConnect);

                    await client.AuthenticateAsync(FromAccount.Address, FromAccount.Password);

                    await client.SendAsync(message);

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