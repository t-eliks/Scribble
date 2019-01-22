namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class CanvasItemModel : BaseModel, ISerializable
    {
        public CanvasItemModel(ICanvasItem item)
        {
            CanvasItem = item;
            item.MarkedForRemoval += (s, e) => { Remove(); };
        }

        private ICanvasItem _CanvasItem;

        public ICanvasItem CanvasItem
        {
            get
            {
                return _CanvasItem;
            }
            set
            {
                if (_CanvasItem != value)
                {
                    _CanvasItem = value;

                    RaisePropertyChanged(nameof(CanvasItem));
                }
            }
        }

        private string _BackgroundColor = "#FF373737";

        public string BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                if (_BackgroundColor != value)
                {
                    _BackgroundColor = value;

                    RaisePropertyChanged(nameof(BackgroundColor));
                }
            }
        }

        private string _ForegroundColor = "#FFF4F4F4";

        public string ForegroundColor
        {
            get
            {
                return _ForegroundColor;
            }
            set
            {
                if (_ForegroundColor != value)
                {
                    _ForegroundColor = value;

                    RaisePropertyChanged(nameof(ForegroundColor));
                }
            }
        }

        private int _Row = 0;

        public int Row
        {
            get
            {
                return _Row;
            }
            set
            {
                if (Row != value)
                {
                    Row = value;

                    RaisePropertyChanged(nameof(Row));
                }
            }
        }

        private double _CanvasLeft = 0;

        public double CanvasLeft
        {
            get
            {
                return _CanvasLeft;
            }
            set
            {
                if (_CanvasLeft != value)
                {
                    _CanvasLeft = value;

                    RaisePropertyChanged(nameof(CanvasLeft));
                }
            }
        }

        private double _CanvasTop = 0;

        public double CanvasTop
        {
            get
            {
                return _CanvasTop;
            }
            set
            {
                if (_CanvasTop != value)
                {
                    _CanvasTop = value;

                    RaisePropertyChanged(nameof(CanvasTop));
                }
            }
        }

        private double _Width = 200.0;

        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                if (_Width != value)
                {
                    _Width = value;

                    RaisePropertyChanged(nameof(Width));
                }
            }
        }

        public virtual void Remove()
        {
            ProjectService.Instance.DeleteItemBiLinks(this);
        }

        protected CanvasItemModel(SerializationInfo info, StreamingContext context)
        {
            CanvasItem = (ICanvasItem)info.GetValue("item", typeof(ICanvasItem));
            CanvasLeft = info.GetDouble("canvasleft");
            CanvasTop = info.GetDouble("canvastop");
            BackgroundColor = info.GetString("backgroundcolor");
            ForegroundColor = info.GetString("foregroundcolor");
            Width = info.GetDouble("width");
            Row = info.GetInt32("row");

            CanvasItem.MarkedForRemoval += (s, e) => {
                Remove();
            };
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("canvasleft", CanvasLeft);
            info.AddValue("canvastop", CanvasTop);
            info.AddValue("item", CanvasItem);
            info.AddValue("backgroundcolor", BackgroundColor);
            info.AddValue("foregroundcolor", ForegroundColor);
            info.AddValue("width", Width);
            info.AddValue("row", Row);
        }
    }
}
