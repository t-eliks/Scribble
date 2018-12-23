namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows;
    using System.Windows.Controls;

    public class MindMapDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Character || item is Scene || item is Location)
                return App.Current.TryFindResource("ItemMindMapTemplate") as DataTemplate;

            return null;
        }
    }
}