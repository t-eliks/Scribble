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
    public class Note : BaseItem, ISerializable, IViewItem
    {
        public Note(Item parent, string name) : base(name, IconHelper.FindIconInResources("Note"))
        {
            Name = name;
            TextFile = new TextFile(ProjectService.Instance?.ActiveProject.FileDirectory);
            Parent = parent;

            TextFile.OnSavedToMainFile += (s, e) => { RaisePropertyChanged(nameof(Preview)); };
        }

        private ICommand _OpenNoteCommand;

        public ICommand OpenNoteCommand
        {
            get
            {
                return _OpenNoteCommand ?? (_OpenNoteCommand = new RelayCommand(() => { OnOpened?.Invoke(this, new RoutedEventArgs()); }));
            }
        }

        private ICommand _MarkForRemovalCommand;

        public ICommand MarkForRemovalCommand
        {
            get
            {
                return _MarkForRemovalCommand ?? (_MarkForRemovalCommand = new RelayCommand(() => 
                {
                    OnMarkedForRemoval?.Invoke(this, new RoutedEventArgs());
                }));
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

        public RoutedEventHandler OnMarkedForRemoval;

        public TextFile TextFile { get; private set; }

        public override void Delete()
        {
            TextFile.Delete();
        }

        #region Serialization

        protected Note(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            TextFile = (TextFile)info.GetValue("textfile", typeof(TextFile));
            Parent = (Item)info.GetValue("parent", typeof(Item));

            TextFile.OnSavedToMainFile += (s, e) => { RaisePropertyChanged(nameof(Preview)); };
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("textfile", TextFile);
            info.AddValue("parent", Parent);
        }

        #endregion
    }
}
