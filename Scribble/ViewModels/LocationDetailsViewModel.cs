namespace Scribble.ViewModels
{
    using Scribble.Models;
    using Scribble.Logic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Scribble.Interfaces;

    public class LocationDetailsViewModel : BaseViewModel, IViewItemViewModel
    {
        public LocationDetailsViewModel()
        {
            
        }

        #region Properties and Fields

        private Location _Location;

        public Location Location
        {
            get
            {
                return _Location;
            }
            set
            {
                if (_Location != value)
                {
                    _Location = value;

                    RaisePropertyChanged(nameof(Location));
                }
            }
        }

        private ICommand _AddNoteCommand;

        public ICommand AddNoteCommand
        {
            get
            {
                return _AddNoteCommand ?? (_AddNoteCommand = new RelayCommand(() =>
                {
                    if (Location != null)
                    {
                        var note = new Note(Location, "New note", null);
                        note.OnOpened += (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };
                        note.OnMarkedForRemoval += (o, a) => { if (Notes.Contains(note)) { Location.Notes.Remove(note); note.Delete(); } };
                        Location.Notes.Add(note);

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

        public ObservableCollection<Scene> Scenes
        {
            get
            {
                return ProjectService.Instance.FindBiLinks<Scene>(Location);
            }
        }

        public ObservableCollection<Note> Notes
        {
            get
            {
                foreach (var note in Location.Notes)
                {
                    //Prevent multiple subscriptions
                    note.OnOpened -= (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };
                    note.OnOpened += (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };

                    note.OnMarkedForRemoval -= (o, a) => { if (Notes.Contains(note)) { Notes.Remove(note); note.Delete(); } };
                    note.OnMarkedForRemoval += (o, a) => { if (Notes.Contains(note)) { Notes.Remove(note); note.Delete(); } };
                }

                return Location.Notes;
            }
        }

        #endregion

    }
}
