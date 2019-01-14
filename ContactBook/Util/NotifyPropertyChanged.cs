using log4net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ContactBook.Util
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        private static readonly ILog log = LogManager.GetLogger("NotifyPropertyChanged");

        protected abstract string ClassName { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            log.DebugFormat("[{0}] Change {1} from '{2}' to '{3}'", ClassName, propertyName, field, value);
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
