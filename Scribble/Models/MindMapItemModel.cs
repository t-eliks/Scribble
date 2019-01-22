namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class MindMapItemModel : CanvasItemModel, ISerializable
    {
        public MindMapItemModel(ICanvasItem item) : base(item)
        {
            CanvasItem = item;
            item.MarkedForRemoval += (s, e) => { Remove(); };
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

        public override void Remove()
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

        protected MindMapItemModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Height = info.GetDouble("height");
            HeaderFontSize = info.GetInt32("headerfontsize");
            ContentFontSize = info.GetInt32("contentfontsize");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("height", Height);
            info.AddValue("headerfontsize", HeaderFontSize);
            info.AddValue("contentfontsize", ContentFontSize);
        }

    }
}
