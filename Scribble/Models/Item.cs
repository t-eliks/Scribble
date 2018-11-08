namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Windows.Input;
    using System.Windows.Media;

    [Serializable]
    public class Item : BaseItem, ISerializable
    {
        public Item() { }

        public Item(string header, ImageSource imageSource)
            : base(header, imageSource)
        {
            
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

        #region Serialization

        protected Item(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
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
