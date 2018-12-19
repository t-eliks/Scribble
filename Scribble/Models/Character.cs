namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
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
        }

        public Character(string name, ImageSource imageSource, string short_name, string description,
            string biography, string goals)
           : base(name, imageSource)
        {
            Biography = biography;
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

        #endregion

        public override bool CheckMatch(string query)
        {
            if (base.CheckMatch(query))
                return true;

            //if (StringHelper.Contains(Short_Name, query) || StringHelper.Contains(Biography, query)
            //    || StringHelper.Contains(Goals, query))
            //    return true;

            return false;
        }

        #region Serialization

        protected Character(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            CharacterType = (CharacterTypes)info.GetValue("charactertype", typeof(CharacterTypes));
            Biography = info.GetString("biography");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("charactertype", CharacterType);
            info.AddValue("biography", Biography);
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