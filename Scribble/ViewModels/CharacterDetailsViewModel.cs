namespace Scribble.ViewModels
{
    using Scribble.Models;
    using Scribble.Logic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Scribble.Interfaces;

    public class CharacterDetailsViewModel : BaseViewModel, IViewItemViewModel
    {
        public CharacterDetailsViewModel()
        {

        }

        private ICommand _AddNoteCommand;

        public ICommand AddNoteCommand
        {
            get
            {
                return _AddNoteCommand ?? (_AddNoteCommand = new RelayCommand(() =>
                {
                    if (Character != null)
                    {
                        var note = new Note(Character, "New note");
                        note.OnOpened += (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };
                        note.OnMarkedForRemoval += (o, a) => { if (Notes.Contains(note)) { Character.Notes.Remove(note); note.Delete(); } };
                        Character.Notes.Add(note);

                        ProjectService.Instance.SaveActiveProject();
                    }
                }));
            }
        }

        private ICommand _RefreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() => { RaisePropertyChanged(nameof(Scenes)); }));
            }
        }

        #region Properties and Fields

        private Character _Character;

        public Character Character
        {
            get
            {
                return _Character;
            }
            set
            {
                if (_Character != value)
                {
                    _Character = value;

                    RaisePropertyChanged(nameof(Character));
                    RaisePropertyChanged(nameof(Major));
                    RaisePropertyChanged(nameof(Minor));
                    RaisePropertyChanged(nameof(Background));
                }
            }
        }

        public bool Major
        {
            get
            {
                return Character.CharacterType == CharacterTypes.Major;
            }
            set
            {
                if (Character.CharacterType != CharacterTypes.Major)
                    Character.CharacterType = CharacterTypes.Major;

                RaisePropertyChanged(nameof(Major));
                RaisePropertyChanged(nameof(Minor));
                RaisePropertyChanged(nameof(Background));
            }
        }

        public bool Minor
        {
            get
            {
                return Character.CharacterType == CharacterTypes.Minor;
            }
            set
            {
                if (Character.CharacterType != CharacterTypes.Minor)
                    Character.CharacterType = CharacterTypes.Minor;

                RaisePropertyChanged(nameof(Major));
                RaisePropertyChanged(nameof(Minor));
                RaisePropertyChanged(nameof(Background));
            }
        }

        public bool Background
        {
            get
            {
                return Character.CharacterType == CharacterTypes.Background;
            }
            set
            {
                if (Character.CharacterType != CharacterTypes.Background)
                    Character.CharacterType = CharacterTypes.Background;

                RaisePropertyChanged(nameof(Major));
                RaisePropertyChanged(nameof(Minor));
                RaisePropertyChanged(nameof(Background));
            }
        }

        public ObservableCollection<Scene> Scenes
        {
            get
            {
                return ProjectService.Instance.FindBiLinks<Scene>(Character);
            }
        }

        public ObservableCollection<Note> Notes
        {
            get
            {
                foreach (var note in Character.Notes)
                {
                    //Prevent multiple subscriptions
                    note.OnOpened -= (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };
                    note.OnOpened += (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };

                    note.OnMarkedForRemoval -= (o, a) => { if (Notes.Contains(note)) { Notes.Remove(note); note.Delete(); } };
                    note.OnMarkedForRemoval += (o, a) => { if (Notes.Contains(note)) { Notes.Remove(note); note.Delete(); } };
                }   

                return Character.Notes;
            }
        }

        #endregion
    }
}
