namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows;
    using System.Windows.Controls;

    public class BulletinBoardDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Character)
                return App.Current.TryFindResource("BulletinCharacter") as DataTemplate;

            if (item is Location)
                return App.Current.TryFindResource("BulletinLocation") as DataTemplate;

            if (item is Scene)
                return App.Current.TryFindResource("BulletinScene") as DataTemplate;

            return null;
        }
    }
}