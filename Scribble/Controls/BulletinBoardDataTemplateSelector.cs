namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows;
    using System.Windows.Controls;

    public class BulletinBoardDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //if (item is Character || item is Scene || item is Location)
            //    return App.Current.TryFindResource("BulletinItem") as DataTemplate;
            if (item is ProjectFolder)
                return App.Current.TryFindResource("BulletinFolder") as DataTemplate;

            return null;
        }
    }
}