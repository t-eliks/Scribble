namespace Scribble.Models
{
    using System.ComponentModel;

    public class BaseModel : INotifyPropertyChanged
    {
        public BaseModel()
        {

        }

        #region INotifyPropertyChanged Impl.

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                //ProjectService.Instance.UnsavedChanges = true;
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }
}
