namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows;
    using System.Windows.Controls;

    public class ListBoxDetailedDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Item)
                return App.Current.TryFindResource("ItemDetailed") as DataTemplate;
            return null;
        }
    }
}
