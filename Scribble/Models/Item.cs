namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Linq;
    using System.Collections.Generic;
    using Scribble.Interfaces;

    [Serializable]
    public class Item : BaseItem, ISerializable, ISearchable
    {
        public Item() { }

        public Item(string name, ImageSource imageSource)
            : base(name, imageSource)
        {
            IsSelected = true;

            Description = "No description.";
        }

        public ObservableCollection<Tag> Tags
        {
            get
            {
                var tags = ProjectService.Instance.FindLinks<Tag>(this);

                tags.CollectionChanged += (s, e) => 
                {
                    foreach (var tag in Tags.Except((IEnumerable<Tag>)s))
                        ProjectService.Instance.DeleteItemLinks(this, tag);

                    foreach (var tag in ((IEnumerable<Tag>)s).Except(Tags))
                        this.AddTag(tag);
                };

                return tags;
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
                if (StringHelper.Contains(_tag.Value, tag))
                    return true;
            }

            return false;
        }

        public void AddTag(Tag tag)
        {
            ProjectService.Instance.AddSymbioticLink(new SymbioticLink<Item, Tag>(this, tag));
        }

        public void AddTag(string tag)
        {
            AddTag(new Tag(tag));
        }

        #region Serialization

        protected Item(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Description = info.GetString("description");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

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

        public virtual bool CheckMatch(string query)
        {
            if (StringHelper.Contains(Name, query) || StringHelper.Contains(Description, query) || this.ContainsTag(query))
                return true;

            return false;
        }

        #endregion
    }

    public enum ItemTypes
    {
        Folder,
        Character,
        Location,
        Scene,
        Timeline
    }
}
