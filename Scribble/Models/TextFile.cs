namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Text;
    using System.Windows;
    using System.Windows.Documents;

    [Serializable]
    public class TextFile : ISerializable
    {
        public TextFile(string directory)
        {
            if (!string.IsNullOrWhiteSpace(directory))
                FilePath = FileService.GenerateEmptyRtf(directory);
        }

        public RoutedEventHandler OnSavedToMainFile;

        private string _FilePath;

        private string FilePath
        {
            get
            {
                return _FilePath != null ? Path.Combine(ProjectService.Instance.ActiveProject.FileDirectory, _FilePath) : null;
            }
            set
            {
                if (_FilePath != value)
                    _FilePath = value;
            }
        }

        private string _BackupPath;

        private string BackupPath
        {
            get
            {
                return _BackupPath != null ? Path.Combine(ProjectService.Instance.ActiveProject.FileDirectory, _BackupPath) : null;
            }
            set
            {
                if (_BackupPath != value)
                    _BackupPath = value;
            }
        }

        private FileStream _Stream;

        public FileStream Stream
        {
            get
            {
                if (_Stream != null && _Stream.CanWrite)
                    return _Stream;
                else
                    return (_Stream = GetStream());
            }
        }

        private FileStream GetStream()
        {

            if (string.IsNullOrWhiteSpace(BackupPath) || !File.Exists(BackupPath))
                BackupPath = Path.Combine(Path.GetDirectoryName(FilePath), $"{Path.GetRandomFileName()}.rtf");

            File.Copy(FilePath, BackupPath, true);
            return new FileStream(BackupPath, FileMode.Open, FileAccess.ReadWrite);
        }

        public void SaveChangesToMainFile()
        {
            if (BackupPath != null && File.Exists(BackupPath))
            {
                File.Copy(BackupPath, FilePath, true);

                OnSavedToMainFile?.Invoke(this, new RoutedEventArgs());
            }
        }

        public void Delete()
        {
            try
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);

                if (File.Exists(BackupPath))
                    File.Delete(BackupPath);
            }
            catch (IOException ex)
            {
                
            }
        }

        #region Serialization

        protected TextFile(SerializationInfo info, StreamingContext context)
        {
            FilePath = info.GetString("filepath");
            BackupPath = info.GetString("backuppath");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("filepath", FilePath);
            info.AddValue("backuppath", BackupPath);
        }

        #endregion
    }
}
