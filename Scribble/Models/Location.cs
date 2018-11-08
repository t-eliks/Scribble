namespace Scribble.Models
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Media;

    [Serializable]
    public class Location : Item, ISerializable
    {
        public Location() { }

        public Location(string header, ImageSource imageSource)
          : base(header, imageSource)
        {
            
        }

        public Location(string header, ImageSource imageSource, string name, string description,
            string details, string notes, string tags)
          : base(header, imageSource)
        {
            Name = name;
            Description = description;
            Details = details;
            Notes = notes;
            Tags = tags;
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

                    RaisePropertyChanged(nameof(Name));
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

        private string _Tags;

        public string Tags
        {
            get
            {
                return _Tags;
            }
            set
            {
                if (_Tags != value)
                {
                    _Tags = value;

                    RaisePropertyChanged(nameof(Tags));
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
            Name = info.GetString("name");
            Description = info.GetString("description");
            Details = info.GetString("details");
            Tags = info.GetString("tags");
            Notes = info.GetString("notes");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("name", Name);
            info.AddValue("description", Description);
            info.AddValue("details", Details);
            info.AddValue("tags", Tags);
            info.AddValue("notes", Notes);
        }

        #endregion
    }
}
