using Prism.Commands;
using Prism.Navigation;

namespace SampleMailer.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Menu";
        }

        /// <summary>
        /// 設定画面へ遷移します。
        /// </summary>
        public DelegateCommand GoToSettingsPageCommand => new DelegateCommand(async () =>
        {
            await NavigationService.NavigateAsync("SettingsPage");
        });

        /// <summary>
        /// メール送信画面へ遷移します。
        /// </summary>
        public DelegateCommand GoToNewMailPageCommand => new DelegateCommand(async () =>
        {
            await NavigationService.NavigateAsync("NewMailPage");
        });
    }
}