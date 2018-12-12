namespace Scribble.Models
{
    using Scribble.Interfaces;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    [Serializable]
    public abstract class BaseItem : BaseModel, ISerializable, IViewItem
    {
        public BaseItem() { }

        public BaseItem(string name, ImageSource imageSource)
        {
            Name = name;
            Image = imageSource;
            ImageSource = imageSource.ToString();

            IsExpanded = true;
        }

        private string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;

                    OnHeaderChanged?.Invoke(this, new RoutedEventArgs());

                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public event RoutedEventHandler OnHeaderChanged;

        public string Header { get { return Name; } }

        private ImageSource _Image;

        public ImageSource Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (_Image != value)
                {
                    _Image = value;

                    RaisePropertyChanged(nameof(Image));
                }
            }
        }

        private string _ImageSource;

        public string ImageSource
        {
            get
            {
                return _ImageSource;
            }
            set
            {
                if (_ImageSource != value)
                {
                    _ImageSource = value;

                    RaisePropertyChanged(nameof(ImageSource));
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

        private bool _IsExpanded;

        public bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                if (_IsExpanded != value)
                {
                    _IsExpanded = value;

                    RaisePropertyChanged(nameof(IsExpanded));
                }
            }
        }

        #region Serialization

        protected BaseItem(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("header");
            ImageSource = info.GetString("_imagesource");
            Image = new BitmapImage(new Uri(ImageSource));

            IsSelected = false;
            IsExpanded = true;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("header", Name);
            info.AddValue("_imagesource", ImageSource);
        }

        #endregion

        #region Methods

        public abstract void Delete();

        public virtual void Collapse()
        {
            IsExpanded = false;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion

    }
}