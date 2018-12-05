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

        public void AddViewItem(IViewItem model, BaseViewModel viewmodel)
        {
            var viewitem = new TabControlViewItem(model, viewmodel);
            viewitem.OnMarkedForRemoval += (s, e) =>
            {
                if (ViewItems != null)
                {
                    if (model is Scene)
                        MainViewModel.OnSceneViewChanged?.Invoke(this, new RoutedEventArgs());
                    ViewItems.Remove(viewitem);
                }
            };

            if (!ViewItems.Contains(viewitem))
                ViewItems.Add(viewitem);
            else
            {
                foreach (var item in ViewItems)
                {
                    if (item.Model == model)
                        item.IsSelected = true;
                }
            }
        }

        public void CloseTab(TabControlViewItem viewitem)
        {
            if (ViewItems.Contains(viewitem))
                ViewItems.Remove(viewitem);
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