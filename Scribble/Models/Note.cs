namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class Note : BaseModel, ISerializable
    {
        public Note()
        {
            Name = "New note";
            TextFile = new TextFile(ProjectService.Instance?.ActiveProject.FileDirectory);
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
                if (value != _Name)
                {
                    _Name = value;

                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public TextFile TextFile { get; private set; }

        #region Serialization

        protected Note(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
            TextFile = (TextFile)info.GetValue("textfile", typeof(TextFile));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("textfile", TextFile);
            info.AddValue("name", Name);
        }

        #endregion
    }
}
