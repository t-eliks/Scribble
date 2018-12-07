namespace Scribble.Interfaces
{
    using System.Windows.Input;

    public interface IViewItemViewModel
    {
        ICommand RefreshCommand { get; }
    }
}