namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Media;

    [Serializable]
    public class Character : Item, ISerializable, ISearchable, IViewItem
    {
        public Character() { }

        public Character(string name, ImageSource imageSource)
           : base(name, imageSource)
        {
            Biography = "No biography.";
            Goals = "No goals.";
            Short_Name = "No short name";
        }

        public Character(string name, ImageSource imageSource, string short_name, string description,
            string biography, string goals)
           : base(name, imageSource)
        {
            Short_Name = short_name;
            Biography = biography;
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

        private ObservableCollection<Note> _Notes;

        public ObservableCollection<Note> Notes
        {
            get
            {
                return _Notes ?? (_Notes = new ObservableCollection<Note>());
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

        public override bool CheckMatch(string query)
        {
            if (base.CheckMatch(query))
                return true;

            if (StringHelper.Contains(Short_Name, query) || StringHelper.Contains(Biography, query)
                || StringHelper.Contains(Goals, query))
                return true;

            return false;
        }

        #region Serialization

        protected Character(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CharacterType = (CharacterTypes)info.GetValue("charactertype", typeof(CharacterTypes));
            Short_Name = info.GetString("shortname");
            Biography = info.GetString("biography");
            Notes = (ObservableCollection<Note>)info.GetValue("notes", typeof(ObservableCollection<Note>));
            Goals = info.GetString("goals");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("charactertype", CharacterType);
            info.AddValue("shortname", Short_Name);
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