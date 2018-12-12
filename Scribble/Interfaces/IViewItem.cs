namespace Scribble.Interfaces
{
    using System.Windows;

    public interface IViewItem
    {
        string Header { get; }

        event RoutedEventHandler OnHeaderChanged;
    }
}