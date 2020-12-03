using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Planeminator.DesktopApp.Core.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
