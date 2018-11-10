namespace Scribble.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Media;

    [Serializable]
    public class Location : Item, ISerializable
    {
        public Location() { }

        public Location(string name, ImageSource imageSource)
          : base(name, imageSource)
        {
        }

        public Location(string name, ImageSource imageSource, string description,
            string details, string notes)
          : base(name, imageSource)
        {
            Details = details;
            Notes = notes;

            Tags = new ObservableCollection<Tag>();
        }

        private string _Details;

        public string Details
        {
            get
            {
                return _Details;
            }
            set
            {
                if (_Details != value)
                {
                    _Details = value;

                    RaisePropertyChanged(nameof(Details));
                }
            }
        }

        private string _Notes;

        public string Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;

                    RaisePropertyChanged(nameof(Notes));
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }

        #region Serialization

        protected Location(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Details = info.GetString("details");
            Notes = info.GetString("notes");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("details", Details);
            info.AddValue("notes", Notes);
        }

        #endregion
    }
}
