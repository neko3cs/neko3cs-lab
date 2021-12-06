using Prism.Commands;
using Prism.Navigation;

namespace Neko3csMailer.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Menu";
        }

        public DelegateCommand GoToSettingsPageCommand => new DelegateCommand(async () =>
        {
            await NavigationService.NavigateAsync("SettingsPage");
        });

        public DelegateCommand GoToNewMailPageCommand => new DelegateCommand(async () =>
        {
            await NavigationService.NavigateAsync("NewMailPage");
        });
    }
}