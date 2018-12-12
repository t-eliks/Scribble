namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows;
    using System.Windows.Controls;

    public class MindMapDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Character c)
                return App.Current.TryFindResource("CharacterOutlineDataTemplate") as DataTemplate;

            if (item is Location l)
                return App.Current.TryFindResource("LocationOutlineDataTemplate") as DataTemplate;

            if (item is Scene s)
                return App.Current.TryFindResource("SceneOutlineDataTemplate") as DataTemplate;

            return null;
        }
    }
}