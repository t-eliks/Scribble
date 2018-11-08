namespace Scribble.Views.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.ViewModels;

    public abstract class DialogViewModelBase<T> : BaseViewModel
    {
        public T DialogResult { get; set; }

        public void CloseDialogWithResult(IDialogWindow dialog, T result)
        {
            DialogResult = result;

            if (dialog != null)
                dialog.DialogResult = true;
        }
    }
}
