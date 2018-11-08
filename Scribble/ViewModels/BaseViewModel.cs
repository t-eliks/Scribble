namespace Scribble.ViewModels
{
    using System.ComponentModel;

    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel() { }

        #region INotifyPropertyChanged Impl.

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }
}
