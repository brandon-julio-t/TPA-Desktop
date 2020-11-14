using System.ComponentModel;
using TPA_Desktop.Properties;

namespace TPA_Desktop.Core.DefaultImplementations
{
    public class DefaultNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged(string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}