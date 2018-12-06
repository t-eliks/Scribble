namespace Scribble.Controls
{
    using Scribble.ViewModels;
    using Scribble.Models;
    using System.Windows.Controls;
    using System.Windows;
    using Scribble.Interfaces;

    public class TabControlViewItem : BaseModel
    {
        public TabControlViewItem(IViewItem model, BaseViewModel viewmodel)
        {
            Model = model;
            Content = new UserControl() { Content = viewmodel };

            IsSelected = true;
        }

        public RoutedEventHandler OnMarkedForRemoval;

        //private ICommand _MarkForRemovalCommand;

        //public ICommand MarkForRemovalCommand
        //{
        //    get
        //    {
        //        return _MarkForRemovalCommand ?? (_MarkForRemovalCommand = new RelayCommand(() =>
        //        { OnMarkedForRemoval?.Invoke(this, new RoutedEventArgs()); }));
        //    }
        //}

        public string Header
        {
            get
            {
                return Model.Header;
            }
        }

        private IViewItem _Model;

        public IViewItem Model
        {
            get
            {
                return _Model;
            }
            set
            {
                if (_Model != value)
                {
                    _Model = value;

                    RaisePropertyChanged(nameof(Model));
                }
            }
        }

        private UserControl _Content;

        public UserControl Content
        {
            get
            {
                return _Content;
            }
            set
            {
                if (_Content != value)
                {
                    _Content = value;

                    RaisePropertyChanged(nameof(Content));
                }
            }
        }

        private bool _IsSelected;

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;

                    RaisePropertyChanged(nameof(IsSelected));
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!ReferenceEquals(this.GetType(), obj.GetType()))
                return false;

            if (this.Model == null || ((TabControlViewItem)obj).Model == null)
            {
                return this.Header == ((TabControlViewItem)obj).Header;
            }
            else
                return this.Model == ((TabControlViewItem)obj).Model;
        }

        public override int GetHashCode()
        {
            return Model.GetHashCode();
        }
    }
}