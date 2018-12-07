namespace Scribble.ViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using System.Windows.Input;

    public class NoteViewModel : BaseViewModel, IViewItemViewModel
    {
        public NoteViewModel()
        {

        }

        private ICommand _RefreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() => { }));
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

        public TextFile TextFile { get { return Note.TextFile; } }
    }
}