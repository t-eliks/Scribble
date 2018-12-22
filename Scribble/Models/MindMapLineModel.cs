namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class MindMapLineModel : BaseModel, ISerializable
    {
        public MindMapLineModel(MindMapItemModel content1, MindMapItemModel content2)
        {
            MindMapContent1 = content1;
            MindMapContent2 = content2;

            ProjectService.Instance.AddSymbioticLink(new SymbioticLink<MindMapItemModel, MindMapLineModel>(content1, this));
            ProjectService.Instance.AddSymbioticLink(new SymbioticLink<MindMapItemModel, MindMapLineModel>(content2, this));

            //Description = "No description.";
            //Header = "New line";
        }

        private MindMapItemModel _MindMapContent1;

        public MindMapItemModel MindMapContent1
        {
            get
            {
                return _MindMapContent1;
            }
            set
            {
                if (_MindMapContent1 != value)
                {
                    _MindMapContent1 = value;

                    RaisePropertyChanged(nameof(MindMapContent1));
                }
            }
        }

        public void Remove()
        {
            ProjectService.Instance.DeleteItemBiLinks(this);
        }

        private MindMapItemModel _MindMapContent2;

        public MindMapItemModel MindMapContent2
        {
            get
            {
                return _MindMapContent2;
            }
            set
            {
                if (_MindMapContent2 != value)
                {
                    _MindMapContent2 = value;

                    RaisePropertyChanged(nameof(MindMapContent2));
                }
            }
        }

        private string _Color = "#FF373737";

        public string Color
        {
            get
            {
                return _Color;
            }
            set
            {
                if (_Color != value)
                {
                    _Color = value;

                    RaisePropertyChanged(nameof(Color));
                }
            }
        }

        public MindMapItemModel GetOpposite(MindMapItemModel model)
        {
            if (MindMapContent1 == model)
                return MindMapContent2;
            if (MindMapContent2 == model)
                return MindMapContent1;

            return null;
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

        private string _Description;

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    _Description = value;

                    RaisePropertyChanged(nameof(Description));
                }
            }
        }

        protected MindMapLineModel(SerializationInfo info, StreamingContext context)
        {
            MindMapContent1 = (MindMapItemModel)info.GetValue("content1", typeof(MindMapItemModel));
            MindMapContent2 = (MindMapItemModel)info.GetValue("content2", typeof(MindMapItemModel));
            Header = info.GetString("header");
            Description = info.GetString("description");
            Color = info.GetString("color");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("content1", MindMapContent1);
            info.AddValue("content2", MindMapContent2);
            info.AddValue("header", Header);
            info.AddValue("description", Description);
            info.AddValue("color", Color);
        }
    }
}
