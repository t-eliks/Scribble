namespace Scribble.Interfaces
{
    using System.Windows;

    public interface ICanvasItem
    {
        string Name { get; set; }

        string Description { get; set; }

        event RoutedEventHandler MarkedForRemoval;
    }
}
