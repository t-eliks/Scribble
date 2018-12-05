namespace Scribble.Models
{
    using Scribble.Controls;
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows;
    using System.Windows.Input;

    [Serializable]
    public class Note : BaseModel, ISerializable, IViewItem
    {
        public Note(Item parent)
        {
            Name = "New note";
            TextFile = new TextFile(ProjectService.Instance?.ActiveProject.FileDirectory);
            Parent = parent;
        }

        private ICommand _OpenNoteCommand;

        public ICommand OpenNoteCommand
        {
            get
            {
                return _OpenNoteCommand ?? (_OpenNoteCommand = new RelayCommand(() => { OnOpened?.Invoke(this, new RoutedEventArgs()); }));
            }
        }

        private string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value != _Name)
                {
                    _Name = value;

                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        public string Preview
        {
            get
            {
                return ExtendedRichTextBox.ParseRTF(TextFile);
            }
        }

        private Item _Parent;

        public Item Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                if (_Parent != value)
                {
                    _Parent = value;

                    RaisePropertyChanged(nameof(Parent));
                }
            }
        }

        public RoutedEventHandler OnOpened;

        public string Header { get { return Name; } }

        public TextFile TextFile { get; private set; }

        #region Serialization

        protected Note(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
            TextFile = (TextFile)info.GetValue("textfile", typeof(TextFile));
            Parent = (Item)info.GetValue("parent", typeof(Item));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("textfile", TextFile);
            info.AddValue("name", Name);
            info.AddValue("parent", Parent);
        }

        #endregion
    }
}
