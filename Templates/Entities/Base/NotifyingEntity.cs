using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Light.Templates.Entities.Base
{
    internal class NotifyingEntity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
