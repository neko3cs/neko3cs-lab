using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace SampleMailer.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; set; }
        private IPageDialogService PageDialogService { get; set; }

        public string Name { get => name; set => SetProperty(ref name, value); }
        private string name;

        public string Address { get => address; set => SetProperty(ref address, value); }
        private string address;

        public string Password { get => password; set => SetProperty(ref password, value); }
        private string password;

        public SettingsPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "Settings";
            NavigationService = navigationService;
            PageDialogService = pageDialogService;

            if (App.Account != null)
            {
                Name = App.Account.Name;
                Address = App.Account.Address;
                Password = App.Account.Password;
            }
        }

        public DelegateCommand SaveUserInfoCommand => new DelegateCommand(async () =>
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Password))
            {
                await PageDialogService.DisplayAlertAsync("There is blank area.", "Please check input area.", "OK");
                return;
            }


            if (App.Account == null)
            {
                App.Account = new UserAccount()
                {
                    ID = 0,
                    Name = Name,
                    Address = Address,
                    Password = Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };
            }
            else
            {
                App.Account.Name = Name;
                App.Account.Address = Address;
                App.Account.Password = Password;
                App.Account.UpdatedAt = DateTime.Now;
            }

            //AccountRepo.SaveUserAccount(Account);
        });
    }
}
