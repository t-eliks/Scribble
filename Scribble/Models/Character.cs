namespace Scribble.Models
{
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Media;

    [Serializable]
    public class Character : Item, ISerializable
    {
        public Character() { }

        public Character(string header, ImageSource imageSource)
           : base(header, imageSource)
        {
            Full_Name = null;
            Short_Name = null;
            Description = null;
            Tags = null;
            Biography = null;
            Notes = null;
            Goals = null;
        }

        public Character(string header, ImageSource imageSource, string full_name, string short_name, string description,
            string tags, string biography, string notes, string goals)
           : base(header, imageSource)
        {
            Full_Name = full_name;
            Short_Name = short_name;
            Description = description;
            Tags = tags;
            Biography = biography;
            Notes = notes;
            Goals = goals;
        }

        #region Properties and Fields

        private CharacterTypes _CharacterType = CharacterTypes.Major;

        public CharacterTypes CharacterType
        {
            get
            {
                return _CharacterType;
            }
            set
            {
                if (_CharacterType != value)
                {
                    _CharacterType = value;

                    RaisePropertyChanged(nameof(CharacterType));
                }
            }
        }

        private string _Full_Name;

        public string Full_Name
        {
            get
            {
                return _Full_Name;
            }
            set
            {
                if (_Full_Name != value)
                {
                    _Full_Name = value;

                    RaisePropertyChanged(nameof(Full_Name));
                }
            }
        }

        private string _Short_Name;

        public string Short_Name
        {
            get
            {
                return _Short_Name;
            }
            set
            {
                if (_Short_Name != value)
                {
                    _Short_Name = value;

                    RaisePropertyChanged(nameof(Short_Name));
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

        private string _Biography;

        public string Biography
        {
            get
            {
                return _Biography;
            }
            set
            {
                if (_Biography != value)
                {
                    _Biography = value;

                    RaisePropertyChanged(nameof(Biography));
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

        private string _Goals;

        public string Goals
        {
            get
            {
                return _Goals;
            }
            set
            {
                if (_Goals != value)
                {
                    _Goals = value;

                    RaisePropertyChanged(nameof(Goals));
                }
            }
        }

        #endregion

        public override string ToString()
        {
            return Full_Name;
        }

        #region Serialization

        protected Character(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CharacterType = (CharacterTypes)info.GetValue("charactertype", typeof(CharacterTypes));
            Full_Name = info.GetString("fullname");
            Short_Name = info.GetString("shortname");
            Description = info.GetString("description");
            Tags = info.GetString("tags");
            Biography = info.GetString("biography");
            Notes = info.GetString("notes");
            Goals = info.GetString("goals");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("charactertype", CharacterType);
            info.AddValue("fullname", Full_Name);
            info.AddValue("shortname", Short_Name);
            info.AddValue("description", Description);
            info.AddValue("tags", Tags);
            info.AddValue("biography", Biography);
            info.AddValue("notes", Notes);
            info.AddValue("goals", Goals);
        }

        #endregion
    }

    public enum CharacterTypes
    {
        Major,
        Minor,
        Background
    }
}