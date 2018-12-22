namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows;
    using System.Windows.Controls;

    public class ListBoxListDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Item)
                return App.Current.TryFindResource("ItemList") as DataTemplate;
            return null;
        }
    }
}
