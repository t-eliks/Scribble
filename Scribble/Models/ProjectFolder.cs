namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Windows.Media;

    [Serializable]
    public class ProjectFolder : BaseItem, ISerializable, IViewItem
    {
        public ProjectFolder() { }

        public ProjectFolder(string name, ImageSource imageSource)
            : base(name, imageSource)
        {
            
        }

        public bool RootFolder { get; set; }

        public ObservableCollection<BaseItem> Content
        {
            get
            {
                return ProjectService.Instance.ActiveProject?.FindLinks<BaseItem>(this);
            }
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public void AddItem(BaseItem item)
        {
            if (!ProjectService.Instance.ActiveProject.FindLinks<BaseItem>(this).Contains(item))
            {
                ProjectService.Instance.ActiveProject.AddSymbioticLink(new SymbioticLink<ProjectFolder, BaseItem>(this, item));
                RaisePropertyChanged(nameof(Content));
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
