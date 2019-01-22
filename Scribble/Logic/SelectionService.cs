namespace Scribble.Logic
{
    using Scribble.Models;
    using Scribble.ViewModels;
    using Scribble.ViewModels.DialogViewModels;
    using System.Collections.Generic;
    using System.Linq;

    public class SelectionService
    {
        public static T SelectItems<T>() where T : BaseItem
        {
            return SelectItems<T>(null, "Select item: ", "Item already added.");
        }

        public static T SelectItems<T>(ICollection<T> SelectedItems, string title, string warning) where T : BaseItem
        {
            var dialog = new SelectViewModel();
            dialog.Collection = ProjectService.Instance.GetItemsOfType<T>();
            if (SelectedItems != null)
                dialog.SelectedItems = SelectedItems.Cast<BaseItem>().ToList();
            dialog.Title = title;
            dialog.Warning = warning;

            var result = MainViewModel._DialogService.OpenDialog(dialog);

            if (result != null)
            {
                return (T)result;
            }

            return null;
        }
    }
}
