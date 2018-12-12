namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Logic.Dialog;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;
    using System.Windows.Input;

    class MindMapInfoViewModel : DialogViewModelBase<DialogResults>
    {
        public MindMapInfoViewModel()
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

        private MindMapModel _MindMap;

        public MindMapModel MindMap
        {
            get
            {
                return _MindMap;
            }
            set
            {
                if (_MindMap != value)
                {
                    _MindMap = value;

                    RaisePropertyChanged(nameof(MindMap));
                }
            }
        }
    }
}
