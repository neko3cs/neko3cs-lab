using System;
using System.Collections.Generic;
using System.Linq;
using MimeKit;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SampleMailer.Models;

namespace SampleMailer.ViewModels
{
    public class NewMailPageViewModel : ViewModelBase
    {
        public string To { get => to; set => SetProperty(ref to, value); }
        private string to;

        public string Cc { get => cc; set => SetProperty(ref cc, value); }
        private string cc;

        public string Bcc { get => bcc; set => SetProperty(ref bcc, value); }
        private string bcc;

        public string Subject { get => subject; set => SetProperty(ref subject, value); }
        private string subject;

        public string Body { get => body; set => SetProperty(ref body, value); }
        private string body;


        public NewMailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            Title = "New Mail";
        }

        public DelegateCommand SendMailCommand => new DelegateCommand(async () =>
        {
            List<MailboxAddress> toList = To.Split(new char[] { ',' }).Select(address => new MailboxAddress(address.Trim())).ToList();
            List<MailboxAddress> ccList = Cc.Split(new char[] { ',' })?.Select(address => new MailboxAddress(address.Trim())).ToList();
            List<MailboxAddress> bccList = Bcc.Split(new char[] { ',' })?.Select(address => new MailboxAddress(address.Trim())).ToList();

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
        });
    }
}
