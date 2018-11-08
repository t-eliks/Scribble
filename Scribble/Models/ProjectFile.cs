namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Windows.Media;

    [Serializable]
    public abstract class ProjectFile : Item, ISerializable
    {
        public ProjectFile() { }

        public ProjectFile(string header, ImageSource imageSource)
            : base(header, imageSource)
        {
            _FileName = CreateFile();
        }

        public virtual string FilePath
        {
            get
            {
                return Path.Combine(ProjectService.Instance.ActiveProject?.FileDirectory, _FileName);
            }
        }

        public override void Delete()
        {
            if (System.IO.Directory.Exists(FilePath))
                System.IO.Directory.Delete(FilePath);

            base.Delete();
        }

        public string _FileName { get; private set; }

        public abstract string CreateFile();

        #region Serialization

        protected ProjectFile(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _FileName = info.GetString("filename");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("filename", _FileName);
        }

        #endregion
    }
}
