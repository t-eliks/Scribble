namespace Scribble.Logic
{
    using Scribble.Models;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public abstract class SymbioticLink
    {
        public abstract X CheckLink<X>(object item);

        public abstract X CheckBiLink<X>(object item);

        public abstract bool IsLink(object parent, object child);

        public abstract bool IsBiLink(object item);

        public abstract X GetItemOfType<X>();

        public abstract object GetObjects(out object child);
    }

    [Serializable]
    public class SymbioticLink<A, B> : SymbioticLink, ISerializable where A : BaseModel where B : BaseModel
    {
        public SymbioticLink(A parent, B child)
        {
            Parent = parent;
            Child = child;
        }

        private A Parent { get; set; }

        private B Child { get; set; }

        public override X CheckLink<X>(object item)
        {
            if ((Parent.Equals(item) && Child is X x))
                return x;

            return default;
        }

        public override X CheckBiLink<X>(object item)
        {
            if ((Parent.Equals(item) && Child is X c))
                return c;
            if ((Child.Equals(item)) && Parent is X p)
                return p;

            return default;
        }

        public override bool IsLink(object parent, object child)
        {
            if ((Parent.Equals(parent)) && Child.Equals(child))
                return true;

            return false;
        }

        public override bool IsBiLink(object item)
        {
            if ((Parent.Equals(item)) || (Child.Equals(item)))
                return true;

            return false;
        }

        public override X GetItemOfType<X>()
        {
            if (Parent is X p)
                return p;
            if (Child is X c)
                return c;

            return default;
        }

        public override object GetObjects(out object child)
        {
            child = Child;

            return Parent;
        }

        #region Serialization

        protected SymbioticLink(SerializationInfo info, StreamingContext context)
        {
            Parent = (A)info.GetValue("parent", typeof(A));
            Child = (B)info.GetValue("child", typeof(B));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("parent", Parent);
            info.AddValue("child", Child);
        }

        #endregion
    }
}
