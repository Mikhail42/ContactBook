using log4net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ContactBook.Util
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        private static readonly ILog log = LogManager.GetLogger("NotifyPropertyChanged");

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            log.DebugFormat("Change {0} to {1}", propertyName, value);
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
