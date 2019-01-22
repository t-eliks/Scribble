namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows;
    using System.Windows.Controls;

    public class TimelineContentDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SceneTemplate { get; set; }

        public DataTemplate LocationTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Scene)
                return SceneTemplate;
            if (item is Location)
                return LocationTemplate;

            return null;
        }
    }
}
