using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Scribble.Models
{
    [Serializable]
    public class Tag : BaseModel, ISerializable
    {
        public Tag()
        {

        }

        public Tag(string tag)
        {
            Value = tag;
        }

        private string _Value;

        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    _Value = value;

                    RaisePropertyChanged(nameof(Value));
                }
            }
        }

        #region Serialization

        protected Tag(SerializationInfo info, StreamingContext context)
        {
            Value = info.GetString("value");
            
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("value", Value);
        }

        #endregion
    }
}
