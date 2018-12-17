namespace Scribble.Interfaces
{
    using System.Windows;

    public interface IViewItem
    {
        string Name { get; }

        event RoutedEventHandler OnHeaderChanged;
    }
}