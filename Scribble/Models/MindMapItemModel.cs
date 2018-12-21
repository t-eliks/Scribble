namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class MindMapItemModel : BaseModel, ISerializable
    {
        public MindMapItemModel(IMindMapItem item)
        {
            MindMapItem = item;
            item.MarkedForRemoval += (s, e) => { Remove(); };
        }

        private IMindMapItem _MindMapItem;

        public IMindMapItem MindMapItem
        {
            get
            {
                return _MindMapItem;
            }
            set
            {
                if (_MindMapItem != value)
                {
                    _MindMapItem = value;

                    RaisePropertyChanged(nameof(MindMapItem));
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

        private bool _HeaderBold = false;

        public bool HeaderBold
        {
            get
            {
                return _HeaderBold;
            }
            set
            {
                if (_HeaderBold != value)
                {
                    _HeaderBold = value;

                    RaisePropertyChanged(nameof(HeaderBold));
                }
            }
        }

        private bool _ContentBold = false;

        public bool ContentBold
        {
            get
            {
                return _ContentBold;
            }
            set
            {
                if (_ContentBold != value)
                {
                    _ContentBold = value;

                    RaisePropertyChanged(nameof(ContentBold));
                }
            }
        }

        public void Remove()
        {
            foreach (var linemodel in Lines)
            {
                linemodel.Remove();
            }

            ProjectService.Instance.DeleteItemBiLinks(this);
        }

        public ObservableCollection<MindMapLineModel> Lines
        {
            get
            {
                return ProjectService.Instance.FindLinks<MindMapLineModel>(this);
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

        private double _Height = 80.0;

        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                if (_Height != value)
                {
                    _Height = value;

                    RaisePropertyChanged(nameof(Height));
                }
            }
        }

        private int _HeaderFontSize = 16;

        public int HeaderFontSize
        {
            get
            {
                return _HeaderFontSize;
            }
            set
            {
                if (_HeaderFontSize != value)
                {
                    _HeaderFontSize = value;

                    RaisePropertyChanged(nameof(HeaderFontSize));
                }
            }
        }

        private int _ContentFontSize = 12;

        public int ContentFontSize
        {
            get
            {
                return _ContentFontSize;
            }
            set
            {
                if (_ContentFontSize != value)
                {
                    _ContentFontSize = value;

                    RaisePropertyChanged(nameof(ContentFontSize));
                }
            }
        }

        public MindMapLineModel GetLine(MindMapItemModel model1, MindMapItemModel model2)
        {
            foreach (var line in Lines)
            {
                if (line.MindMapContent1 == model1 && line.MindMapContent2 == model2)
                    return line;
                if (line.MindMapContent2 == model1 && line.MindMapContent1 == model2)
                    return line;
            }

            return null;
        }

        public bool ContainsLine(MindMapItemModel model1, MindMapItemModel model2)
        {
            foreach (var line in Lines)
            {
                return (line.MindMapContent1 == model1 && line.MindMapContent2 == model2) || (line.MindMapContent1 == model2 && line.MindMapContent2 == model1);
            }

            return false;
        }

        protected MindMapItemModel(SerializationInfo info, StreamingContext context)
        {
            MindMapItem = (IMindMapItem)info.GetValue("item", typeof(IMindMapItem));
            CanvasLeft = info.GetDouble("canvasleft");
            CanvasTop = info.GetDouble("canvastop");
            BackgroundColor = info.GetString("backgroundcolor");
            ForegroundColor = info.GetString("foregroundcolor");
            Height = info.GetDouble("height");
            Width = info.GetDouble("width");
            HeaderFontSize = info.GetInt32("headerfontsize");
            ContentFontSize = info.GetInt32("contentfontsize");

            MindMapItem.MarkedForRemoval += (s, e) => {
                ProjectService.Instance.DeleteItemBiLinks(this);
                foreach (var linemodel in Lines)
                {
                    ProjectService.Instance.DeleteItemBiLinks(linemodel);
                }
            };
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("canvasleft", CanvasLeft);
            info.AddValue("canvastop", CanvasTop);
            info.AddValue("item", MindMapItem);
            info.AddValue("backgroundcolor", BackgroundColor);
            info.AddValue("foregroundcolor", ForegroundColor);
            info.AddValue("height", Height);
            info.AddValue("width", Width);
            info.AddValue("headerfontsize", HeaderFontSize);
            info.AddValue("contentfontsize", ContentFontSize);
        }

    }
}
