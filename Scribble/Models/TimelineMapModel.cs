namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class TimelineMapModel : BaseItem, ISerializable
    {
        public TimelineMapModel(string name) : base(name, IconHelper.FindIconInResources("Timeline"))
        {

        }

        public ObservableCollection<TimelineModel> MapContent
        {
            get
            {
                return ProjectService.Instance.FindLinks<TimelineModel>(this);
            }
        }

        private string _Description = "No description.";

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

        protected TimelineMapModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Description = info.GetString("description");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("description", Description);
        }
    }
}
