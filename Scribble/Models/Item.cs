namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Input;
    using System.Windows.Media;

    [Serializable]
    public class Item : BaseItem, ISerializable
    {
        public Item() { }

        public Item(string name, ImageSource imageSource)
            : base(name, imageSource)
        {
            IsSelected = true;

            Description = "No description.";

            Tags = new ObservableCollection<Tag>();
        }

        private ObservableCollection<Tag> _Tags;

        public ObservableCollection<Tag> Tags
        {
            get
            {
                return _Tags;
            }
            set
            {
                if (_Tags != value)
                {
                    _Tags = value;

                    RaisePropertyChanged(nameof(Tags));
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

        private ICommand _ToggleIsSelectedCommand;

        public ICommand ToggleIsSelectedCommand
        {
            get
            {
                return _ToggleIsSelectedCommand ?? (_ToggleIsSelectedCommand = new RelayCommand(() => { IsSelected = true; }));
            }
        }

        public ObservableCollection<Item> Items
        {
            get
            {
                return ProjectService.Instance.ActiveProject?.FindLinks<Item>(this);
            }
        }

        public override void Delete()
        {
            ProjectService.Instance.ActiveProject?.DeleteItemBiLinks(this);
        }

        public bool ContainsTag(string tag)
        {
            foreach (var _tag in Tags)
            {
                if (_tag.Value == tag)
                    return true;
            }

            return false;
        }

        #region Serialization

        protected Item(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Tags = (ObservableCollection<Tag>)info.GetValue("tags", typeof(ObservableCollection<Tag>));
            Description = info.GetString("description");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("tags", Tags);
            info.AddValue("description", Description);
        }

        public virtual void AddItem(Item item)
        {
            if (!ProjectService.Instance.ActiveProject.FindLinks<Item>(this).Contains(item))
            {
                ProjectService.Instance.ActiveProject.AddSymbioticLink(new SymbioticLink<Item, Item>(this, item));
                RaisePropertyChanged(nameof(Items));
            }
        }

        public virtual void DeleteItem(Item item)
        {
            ProjectService.Instance.ActiveProject?.DeleteItemLinks(this, item);
        }

        #endregion
    }
}
