namespace Scribble.ViewModels
{
    using Scribble.Models;

    public class NoteViewModel : BaseViewModel
    {
        public NoteViewModel()
        {

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