namespace Scribble.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Media;

    [Serializable]
    public class MindMapItemModel : BaseModel, ISerializable
    {
        public MindMapItemModel(BaseItem item)
        {
            MindMapItem = item;
        }

        private BaseItem _MindMapItem;

        public BaseItem MindMapItem
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

        private MindMapItemColors _BackgroundColor = MindMapItemColors.Charcoal;

        public MindMapItemColors BackgroundColor
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

        private ObservableCollection<MindMapLineModel> _Lines;

        public ObservableCollection<MindMapLineModel> Lines
        {
            get
            {
                return _Lines ?? (_Lines = new ObservableCollection<MindMapLineModel>());
            }
            set
            {
                if (_Lines != value)
                {
                    _Lines = value;

                    RaisePropertyChanged(nameof(Lines));
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
            MindMapItem = (BaseItem)info.GetValue("item", typeof(BaseItem));
            Lines = (ObservableCollection<MindMapLineModel>)info.GetValue("lines", typeof(ObservableCollection<MindMapLineModel>));
            CanvasLeft = info.GetDouble("canvasleft");
            CanvasTop = info.GetDouble("canvastop");
            BackgroundColor = (MindMapItemColors)info.GetValue("backgroundcolor", typeof(MindMapItemColors));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("canvasleft", CanvasLeft);
            info.AddValue("canvastop", CanvasTop);
            info.AddValue("lines", Lines);
            info.AddValue("item", MindMapItem);
            info.AddValue("backgroundcolor", BackgroundColor);
        }

        public static SolidColorBrush GetBrush(MindMapItemColors color)
        {
            switch (color)
            {
                case MindMapItemColors.Vermillion:
                    return new SolidColorBrush(Color.FromRgb(252, 74, 26));
                case MindMapItemColors.Charcoal:
                    return new SolidColorBrush(Color.FromRgb(55, 55, 55));
                case MindMapItemColors.Paper:
                    return new SolidColorBrush(Color.FromRgb(244, 244, 244));
                case MindMapItemColors.PaleGold:
                    return new SolidColorBrush(Color.FromRgb(192, 178, 131));
                case MindMapItemColors.Teal:
                    return new SolidColorBrush(Color.FromRgb(7, 136, 155));
                case MindMapItemColors.Void:
                    return new SolidColorBrush(Color.FromRgb(14, 11, 22));
                case MindMapItemColors.Jewel:
                    return new SolidColorBrush(Color.FromRgb(71, 23, 246));
                case MindMapItemColors.Overcast:
                    return new SolidColorBrush(Color.FromRgb(144, 153, 162));
                case MindMapItemColors.Ink:
                    return new SolidColorBrush(Color.FromRgb(6, 47, 79));
                case MindMapItemColors.Embers:
                    return new SolidColorBrush(Color.FromRgb(184, 38, 1));
                case MindMapItemColors.Watermelon:
                    return new SolidColorBrush(Color.FromRgb(255, 59, 63));
            }

            return null;
        }

    }

    public enum MindMapItemColors
    {
        Vermillion,
        Charcoal,
        Paper,
        PaleGold,
        Teal,
        Void,
        Jewel,
        Overcast,
        Ink,
        Embers,
        Watermelon
    }
}
