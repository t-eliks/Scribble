namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Windows.Media;

    [Serializable]
    public class ProjectFolder : Item, ISerializable
    {
        public ProjectFolder() { }

        public ProjectFolder(string name, ImageSource imageSource)
            : base(name, imageSource)
        {
            
        }

        public bool RootFolder { get; set; }

        public ObservableCollection<Item> Content
        {
            get
            {
                return ProjectService.Instance.ActiveProject?.FindLinks<Item>(this);
            }
        }

        #region Serialization

        protected ProjectFolder(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            RootFolder = info.GetBoolean("root");

            if (RootFolder)
                IsSelected = true;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("root", RootFolder);
        }

        #endregion
    }
}
