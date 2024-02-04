using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MassTransit.Message.Broker.ViewModels;

internal abstract class ViewModelBase : INotifyPropertyChanged
{

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

}
