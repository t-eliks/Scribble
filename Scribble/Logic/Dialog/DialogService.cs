namespace Scribble.Logic.Dialog
{
    using Scribble.Views.DialogViewModels;
    using Scribble.Interfaces;
    using Scribble.Views.DialogViews;

    public class DialogService
    {
        public T OpenDialog<T>(DialogViewModelBase<T> viewModel)
        {
            IDialogWindow window = new BaseDialogWindow();
            window.DataContext = viewModel;
            window.ShowDialog();
            return viewModel.DialogResult;
        }
    }
}
