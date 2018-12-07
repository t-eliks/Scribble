namespace Scribble.Logic
{
    using Scribble.Controls;
    using Scribble.Interfaces;
    using Scribble.Models;
    using Scribble.ViewModels;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    public class ViewItemService : BaseModel
    {
        private ViewItemService()
        {

        }

        public static ViewItemService Instance { get; } = new ViewItemService();

        private ObservableCollection<TabControlViewItem> _ViewItems;

        public ObservableCollection<TabControlViewItem> ViewItems
        {
            get
            {
                return _ViewItems ?? (_ViewItems = new ObservableCollection<TabControlViewItem>());
            }
            set
            {
                if (_ViewItems != value)
                {
                    _ViewItems = value;

                    RaisePropertyChanged(nameof(ViewItems));
                }
            }
        }

        public void AddViewItem(IViewItem model, IViewItemViewModel viewmodel)
        {
            var viewitem = new TabControlViewItem(model, viewmodel);

            _AddViewItem(viewitem);
        }

        public void AddViewItem(string header, IViewItemViewModel viewmodel)
        {
            var viewitem = new TabControlViewItem(header, viewmodel);

            _AddViewItem(viewitem);
        }

        private void _AddViewItem(TabControlViewItem viewitem)
        {
            if (!ViewItems.Contains(viewitem))
                ViewItems.Add(viewitem);
            else
            {
                foreach (var item in ViewItems)
                {
                    if (item.Model == viewitem.Model)
                        item.IsSelected = true;
                }
            }
        }

        public void CloseTab(TabControlViewItem viewitem)
        {
            if (ViewItems.Contains(viewitem))
                ViewItems.Remove(viewitem);
        }

        public void CloseTab(BaseItem item)
        {
            foreach (var viewitem in ViewItems.ToList())
            {
                if (viewitem.Model == item)
                    CloseTab(viewitem);
            }
        }

        public void MakeActive(BaseItem item)
        {
            foreach (var viewitem in ViewItems)
            {
                if (viewitem.Model == item)
                    viewitem.IsSelected = true;
            }
        }

        public void CloseOtherTabs(TabControlViewItem viewitem)
        {
            if (ViewItems.Contains(viewitem))
            {
                foreach (var item in ViewItems.ToList())
                {
                    if (item != viewitem)
                        ViewItems.Remove(item);
                }
            }
        }
    }
}