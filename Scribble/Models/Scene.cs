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

        private double _CanvasLeft = 0;

        public double CanvasLeft
        {
            get
            {
                return _CanvasLeft;
            }
            set
            {
                if (_CanvasLeft != value)
                {
                    _CanvasLeft = value;

                    RaisePropertyChanged(nameof(CanvasLeft));
                }
            }
        }

        private double _CanvasTop = 0;

        public double CanvasTop
        {
            get
            {
                return _CanvasTop;
            }
            set
            {
                if (_CanvasTop != value)
                {
                    _CanvasTop = value;

                    RaisePropertyChanged(nameof(CanvasTop));
                }
            }
        }

        public bool IsInTimeline { get; set; }

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
            CanvasLeft = info.GetDouble("canvasleft");
            CanvasTop = info.GetDouble("canvastop");
            IsInTimeline = info.GetBoolean("isintimeline");
            TextFile = (TextFile)info.GetValue("textfile", typeof(TextFile));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("canvasleft", CanvasLeft);
            info.AddValue("canvastop", CanvasTop);
            info.AddValue("isintimeline", IsInTimeline);
            info.AddValue("textfile", TextFile);
        }

        #endregion

    }
}
