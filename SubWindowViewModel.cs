using PropertyChanged;
using Reactive.Bindings;
using RedSheeps.Wpf.Interactivity;
using System;
using System.Windows;

namespace SystemWindowsInteractivitySample
{
    [AddINotifyPropertyChangedInterface]
    public class SubWindowViewModel
    {
        public INotification<ShowMessageEventArgs> ConfirmReceiveNotification { get; } = new Notification<ShowMessageEventArgs>();
        public INotification RequestCloseNotification { get; } = new Notification();
        public ReactiveCommand CloseWindowCommand { get; } = new ReactiveCommand();

        public SubWindowViewModel()
        {
            CloseWindowCommand.Subscribe(OnCloseWindow);
        }

        private void OnCloseWindow(object _)
        {
            var confirmArgs = new ShowMessageEventArgs();
            ConfirmReceiveNotification.Notify(confirmArgs);
            if (confirmArgs.MessageBoxResult is MessageBoxResult.Cancel) return;

            RequestCloseNotification.Notify();
        }
    }
}
