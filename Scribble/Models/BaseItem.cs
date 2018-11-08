namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    [Serializable]
    public abstract class BaseItem : BaseModel, ISerializable
    {
        public BaseItem() { }

        public BaseItem(string header, ImageSource imageSource)
        {
            Header = header;
            Image = imageSource;
            ImageSource = imageSource.ToString();

            IsExpanded = true;
        }

        private string _Header;

        public string Header
        {
            get
            {
                return _Header;
            }
            set
            {
                if (_Header != value)
                {
                    _Header = value;

                    RaisePropertyChanged(nameof(Header));
                }
            }
        }

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
            Header = info.GetString("header");
            ImageSource = info.GetString("_imagesource");
            Image = new BitmapImage(new Uri(ImageSource));

            IsSelected = false;
            IsExpanded = true;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("header", Header);
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
            return Header;
        }

        #endregion

    }
}