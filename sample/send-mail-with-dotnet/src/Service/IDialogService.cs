namespace SendMailWithDotnet.Service;

public interface IDialogService
{
  Task DisplayAlertAsync(string title, string message, string cancel);
}