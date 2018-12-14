namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Logic.Dialog;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;
    using System.Windows.Input;

    public class MindMapStringEditViewModel : DialogViewModelBase<DialogResults>
    {
        public MindMapStringEditViewModel()
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

        private MindMapString _MindMapString;

        public MindMapString MindMapString
        {
            get
            {
                return _MindMapString;
            }
            set
            {
                if (_MindMapString != value)
                {
                    _MindMapString = value;

                    RaisePropertyChanged(nameof(MindMapString));
                }
            }
        }
    }
}
