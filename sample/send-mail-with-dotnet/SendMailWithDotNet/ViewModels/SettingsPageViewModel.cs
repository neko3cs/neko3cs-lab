using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SendMailWithDotNet.Models;

namespace SendMailWithDotNet.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    [field: ObservableProperty]
    [field: NotifyPropertyChangedFor(nameof(Name))]
    public string Name { get; private set; }

    [field: ObservableProperty]
    [field: NotifyPropertyChangedFor(nameof(Address))]
    public string Address { get; private set; }

    [field: ObservableProperty]
    [field: NotifyPropertyChangedFor(nameof(Password))]
    public string Password { get; private set; }

    [RelayCommand]
    public void SaveUserInfoCommand()
    {
        if (
            string.IsNullOrEmpty(Name) ||
            string.IsNullOrEmpty(Address) ||
            string.IsNullOrEmpty(Password)
        )
        {
            // await PageDialogService.DisplayAlertAsync("There is blank area.", "Please check input area.", "OK");
            return;
        }

        App.Account = new Account(
            Name,
            Address,
            Password
        );
    }
}