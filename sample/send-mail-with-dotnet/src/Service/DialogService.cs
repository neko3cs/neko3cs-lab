namespace SendMailWithDotnet.Service;

public class DialogService : IDialogService
{
  public async Task DisplayAlertAsync(string title, string message, string buttonTitle)
  {
    await Application.Current.MainPage.DisplayAlert(title, message, buttonTitle);
  }
}