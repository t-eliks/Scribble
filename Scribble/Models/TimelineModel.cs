namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class TimelineModel : BaseModel, ISerializable
    {
        public TimelineModel()
        {

        }

        public ObservableCollection<CanvasItemModel> Content
        {
            get
            {
                return ProjectService.Instance.FindLinks<CanvasItemModel>(this);
            }
        }

        public void AddContent(CanvasItemModel itemmodel)
        {
            ProjectService.Instance.AddSymbioticLink(new SymbioticLink<TimelineModel, CanvasItemModel>(this, itemmodel));

            RaisePropertyChanged(nameof(Content));
        }

        private string _Name = "New timeline.";

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

                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        private int _Rows = 2;

        public int Rows
        {
            get
            {
                return _Rows;
            }
            set
            {
                if (_Rows != value && value >= 0)
                {
                    _Rows = value;

                    RaisePropertyChanged(nameof(Rows));
                }
            }
        }

        protected TimelineModel(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
            Rows = info.GetInt32("rows");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("rows", Rows);
        }

    }
}
