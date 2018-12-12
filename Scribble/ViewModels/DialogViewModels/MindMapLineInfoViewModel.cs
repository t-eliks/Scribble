namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Controls;
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Logic.Dialog;
    using Scribble.Views.DialogViewModels;
    using System.Windows.Input;

    public class MindMapLineInfoViewModel : DialogViewModelBase<DialogResults>
    {
        public MindMapLineInfoViewModel()
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

        private MindMapLine _MindMapLine;

        public MindMapLine MindMapLine
        {
            get
            {
                return _MindMapLine;
            }
            set
            {
                if (_MindMapLine != value)
                {
                    _MindMapLine = value;

                    RaisePropertyChanged(nameof(MindMapLine));
                }
            }
        }

    }
}
