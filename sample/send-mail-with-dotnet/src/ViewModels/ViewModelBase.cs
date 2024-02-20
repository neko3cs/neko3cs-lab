using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SendMailWithDotnet.ViewModels;

public class ViewModelBase : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler PropertyChanged;

  protected virtual void SetProperty<T>(
    ref T field,
    T value,
    [CallerMemberName] string propertyName = null)
  {
    if (!Equals(field, value))
    {
      field = value;
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}