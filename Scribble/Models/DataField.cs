namespace Scribble.Models
{
    using Scribble.Interfaces;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class DataField : BaseModel, ITwoField, ISerializable
    {
        public DataField(string name)
        {
            Name = name;
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

        private string _Content;

        public string Content
        {
            get
            {
                return _Content;
            }
            set
            {
                if (_Content != value)
                {
                    _Content = value;

                    RaisePropertyChanged(nameof(Content));
                }
            }
        }

        public string Description { get { return Content; } set { Content = value; } }

        #region Serialization

        protected DataField(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
            Content = info.GetString("content");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("content", Content);
        }

        #endregion
    }
}
