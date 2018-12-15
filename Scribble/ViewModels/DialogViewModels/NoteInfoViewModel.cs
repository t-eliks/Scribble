namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Logic.Dialog;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;
    using System.Windows.Input;

    public class NoteInfoViewModel : DialogViewModelBase<DialogResults>
    {
        public NoteInfoViewModel()
        {

        }

        private ICommand _CloseCommand;

        public ICommand CloseCommand
        {
            get
            {
                return _CloseCommand ?? (_CloseCommand = new RelayCommand<IDialogWindow>((window) => { CloseDialogWithResult(window, DialogResults.Yes); }));
            }
        }

        private Note _Note;

        public Note Note
        {
            get
            {
                return _Note;
            }
            set
            {
                if (_Note != value)
                {
                    _Note = value;

                    RaisePropertyChanged(nameof(Note));
                }
            }
        }
    }
}
