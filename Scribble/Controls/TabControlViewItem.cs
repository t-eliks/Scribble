namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows.Controls;
    using Scribble.Interfaces;

    public class TabControlViewItem : BaseModel
    {
        public TabControlViewItem(IViewItem model, IViewItemViewModel viewmodel)
        {
            Model = model;
            Content = new UserControl() { Content = viewmodel };

            Model.OnHeaderChanged += (s, e) => { RaisePropertyChanged(nameof(Header)); };
            IsSelected = true;
        }

        public TabControlViewItem(string header, IViewItemViewModel viewmodel)
        {
            Content = new UserControl() { Content = viewmodel };

            _Header = header;

            IsSelected = true;
        }

        private string _Header;

        public string Header
        {
            get
            {
                return Model != null ? Model.Name : _Header;
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
            if (Model != null)
                return Model.GetHashCode();
            else
                return _Header.GetHashCode();
        }
    }
}