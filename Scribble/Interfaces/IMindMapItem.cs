namespace Scribble.Interfaces
{
    using System.Windows;

    public interface IMindMapItem
    {
        string Name { get; set; }

        string Description { get; set; }

        event RoutedEventHandler MarkedForRemoval;
    }
}
