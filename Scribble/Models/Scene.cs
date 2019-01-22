namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Media;

    [Serializable]
    public class Scene : Item, ISerializable, ISearchable, IViewItem
    {
        public Scene() { }

        public Scene(string name, ImageSource imageSource)
            : base(name, imageSource)
        {
            TextFile = new TextFile(ProjectService.Instance.ActiveProject?.FileDirectory);
        }

        public TextFile TextFile { get; private set; }

        public override void Delete()
        {
            TextFile.Delete();

            base.Delete();
        }

        public override bool CheckMatch(string query)
        {
            if (base.CheckMatch(query))
                return true;

            //if (StringHelper.Contains(Role, query) || StringHelper.Contains(Sights, query) || StringHelper.Contains(Sounds, query) 
            //    || StringHelper.Contains(Smells, query) || StringHelper.Contains(Outcome, query))
            //    return true;

            return false;
        }

        #region Serialization

        protected Scene(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            TextFile = (TextFile)info.GetValue("textfile", typeof(TextFile));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("textfile", TextFile);
        }

        #endregion

    }
}
