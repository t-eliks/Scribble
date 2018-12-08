namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Media;

    [Serializable]
    public class Location : Item, ISerializable, ISearchable, IViewItem
    {
        public Location() { }

        public Location(string name, ImageSource imageSource)
          : base(name, imageSource)
        {
            Details = "No details.";
        }

        public Location(string name, ImageSource imageSource, string description,
            string details)
          : base(name, imageSource)
        {
            Details = details;
        }

        private string _Details;

        public string Details
        {
            get
            {
                return _Details;
            }
            set
            {
                if (_Details != value)
                {
                    _Details = value;

                    RaisePropertyChanged(nameof(Details));
                }
            }
        }

        public override bool CheckMatch(string query)
        {
            if (base.CheckMatch(query))
                return true;

            if (StringHelper.Contains(Details, query))
                return true;

            return false;
        }

        #region Serialization

        protected Location(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Details = info.GetString("details");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("details", Details);
        }

        #endregion
    }
}
