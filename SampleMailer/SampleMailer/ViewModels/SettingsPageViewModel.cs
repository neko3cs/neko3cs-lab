using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SampleMailer.Models;

namespace SampleMailer.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public string Name { get => name; set => SetProperty(ref name, value); }
        private string name;

        public string Address { get => address; set => SetProperty(ref address, value); }
        private string address;

        public string Password { get => password; set => SetProperty(ref password, value); }
        private string password;


        public SettingsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            Title = "Settings";

            if (App.Account != null)
            {
                Name = App.Account.Name;
                Address = App.Account.Address;
                Password = App.Account.Password;
            }
        }

        /// <summary>
        /// ユーザ情報を保存します。
        /// </summary>
        public DelegateCommand SaveUserInfoCommand => new DelegateCommand(async () =>
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Password))
            {
                await PageDialogService.DisplayAlertAsync("There is blank area.", "Please check input area.", "OK");
                return;
            }

            if (App.Account == null)
            {
                App.Account = new Account()
                {
                    Name = Name,
                    Address = Address,
                    Password = Password
                };
            }

            if (Address.Equals(App.Account.Address) && Password.Equals(App.Account.Password)) { return; }

            App.Account.Name = Name;
            App.Account.Address = Address;
            App.Account.Password = Password;
        });
    }
}