namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class MindMapString : BaseItem, ISerializable, ITwoField, IMindMapItem
    {
        public MindMapString(string header, string content) : base(header, IconHelper.FindIconInResources("Note"))
        {
            Content = content;
        }

        public string Description
        {
            get
            {
                return Content;
            }
            set
            {
                Content = value;
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
                    RaisePropertyChanged(nameof(Description));
                }
            }
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        #region Serialization

        protected MindMapString(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Content = info.GetString("content");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("content", Content);
        }

        #endregion

    }
}
