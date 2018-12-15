namespace Scribble.Models
{
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
        public Note(Item parent, string name, string description) : base(name, IconHelper.FindIconInResources("Note"))
        {
            Name = name;
            TextFile = new TextFile(ProjectService.Instance?.ActiveProject.FileDirectory);
            Parent = parent;
            Description = description;

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

        private string _Description;

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    _Description = value;

                    RaisePropertyChanged(nameof(Description));
                }
            }
        }

        public RoutedEventHandler OnOpened;

        public RoutedEventHandler OnMarkedForRemoval;

        public TextFile TextFile { get; private set; }

        public override void Delete()
        {
            base.Delete();

            TextFile.Delete();
        }

        #region Serialization

        protected Note(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            TextFile = (TextFile)info.GetValue("textfile", typeof(TextFile));
            Parent = (Item)info.GetValue("parent", typeof(Item));
            Description = info.GetString("description");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("textfile", TextFile);
            info.AddValue("parent", Parent);
            info.AddValue("description", Description);
        }

        #endregion
    }
}
