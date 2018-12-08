namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows;
    using System.Windows.Controls;

    public class DefaultDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //if (item is Character)
            //    return App.Current.TryFindResource("CharactersListBoxItemTemplate") as DataTemplate;

            //if (item is Location)
            //    return App.Current.TryFindResource("LocationsListBoxItemTemplate") as DataTemplate;

            if (item is Scene)
                return App.Current.TryFindResource("SceneDataTemplate") as DataTemplate;

            return null;
        }
    }
}
