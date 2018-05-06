using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SampleMailer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleMailer.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; set; }


        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Menu";
            NavigationService = navigationService;
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
