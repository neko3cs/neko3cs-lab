using MimeKit;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SampleMailer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleMailer.ViewModels
{
    public class NewMailPageViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; set; }

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

        public NewMailPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "New Mail";
            NavigationService = navigationService;
        }

        public DelegateCommand SendMailCommand => new DelegateCommand(() =>
        {
            List<MailboxAddress> toList = To.Split(new char[] { ',' }).Select(address => new MailboxAddress(address.Trim())).ToList();
            List<MailboxAddress> ccList = Cc.Split(new char[] { ',' }).Select(address => new MailboxAddress(address.Trim())).ToList();
            List<MailboxAddress> bccList = Bcc.Split(new char[] { ',' }).Select(address => new MailboxAddress(address.Trim())).ToList();

            var mailer = new Mailer(App.Account);
            mailer.Send(toList, ccList, bccList, Subject, Body);
        });
    }
}
