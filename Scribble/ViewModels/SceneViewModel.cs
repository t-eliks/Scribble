namespace Scribble.ViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.ViewModels.DialogViewModels;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class SceneViewModel : BaseViewModel, IViewItemViewModel
    {
        public SceneViewModel()
        {

        }

        private ICommand _RemoveCharacterCommand;

        public ICommand RemoveCharacterCommand
        {
            get
            {
                return _RemoveCharacterCommand ?? (_RemoveCharacterCommand = new RelayCommand(() => 
                {
                    if (SelectedItem != null && SelectedItem is Character character && CharactersInScene.Contains(character))
                    {
                        Scene.Items.Remove(character);

                        RaisePropertyChanged(nameof(CharactersInScene));
                    }
                }));
            }
        }

        private ICommand _AddCharacterCommand;

        public ICommand AddCharacterCommand
        {
            get
            {
                return _AddCharacterCommand ?? (_AddCharacterCommand = new RelayCommand(() => 
                {
                    var dialog = new SelectViewModel();
                    dialog.Collection = new ObservableCollection<BaseItem>(ProjectService.Instance.GetItemsOfType<Character>().Cast<BaseItem>());
                    dialog.SelectedItems = new ObservableCollection<BaseItem>(CharactersInScene.Cast<BaseItem>());
                    dialog.Title = "All characters in project";
                    dialog.Warning = "Character already in scene.";
                    dialog.Button_Text = "Add character";

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        Scene.AddItem((Item)result);

                        RaisePropertyChanged(nameof(CharactersInScene));
                    }
                } ));
            }
        }


        private ICommand _AddNoteCommand;

        public ICommand AddNoteCommand
        {
            get
            {
                return _AddNoteCommand ?? (_AddNoteCommand = new RelayCommand(() =>
                {
                    if (Scene != null)
                    {
                        var note = new Note(Scene, "New note", null);
                        note.OnMarkedForRemoval += (o, a) => { if (Notes.Contains(note)) { Scene.Notes.Remove(note); note.Delete(); } };
                        Scene.Notes.Add(note);

                        ProjectService.Instance.SaveActiveProject();
                    }
                }));
            }
        }

        public ObservableCollection<Note> Notes
        {
            get
            {
                foreach (var note in Scene.Notes)
                {
                    //Prevent multiple subscriptions
                    note.OnOpened -= (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };
                    note.OnOpened += (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };

                    note.OnMarkedForRemoval -= (o, a) => { if (Notes.Contains(note)) { Notes.Remove(note); note.Delete(); } };
                    note.OnMarkedForRemoval += (o, a) => { if (Notes.Contains(note)) { Notes.Remove(note); note.Delete(); } };
                }

                return Scene.Notes;
            }
        }

        private ICommand _RemoveLocationCommand;

        public ICommand RemoveLocationCommand
        {
            get
            {
                return _RemoveLocationCommand ?? (_RemoveLocationCommand = new RelayCommand(() =>
                {
                    if (SelectedItem != null && SelectedItem is Location location && LocationsInScene.Contains(location))
                    {
                        Scene.Items.Remove(location);

                        RaisePropertyChanged(nameof(LocationsInScene));
                    }
                }));
            }
        }

        private ICommand _AddLocationCommand;

        public ICommand AddLocationCommand
        {
            get
            {
                return _AddLocationCommand ?? (_AddLocationCommand = new RelayCommand(() =>
                {
                    var dialog = new SelectViewModel();
                    dialog.Collection = new ObservableCollection<BaseItem>(ProjectService.Instance.GetItemsOfType<Location>().Cast<BaseItem>());
                    dialog.SelectedItems = new ObservableCollection<BaseItem>(LocationsInScene.Cast<BaseItem>());
                    dialog.Title = "All locations in project";
                    dialog.Warning = "Location already in scene.";
                    dialog.Button_Text = "Add location";

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        Scene.AddItem((Item)result);

                        RaisePropertyChanged(nameof(LocationsInScene));
                    }
                }));
            }
        }

        private ICommand _RefreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() => { RaisePropertyChanged(nameof(CharactersInScene)); RaisePropertyChanged(nameof(LocationsInScene)); }));
            }
        }

        private Scene _Scene;

        public Scene Scene
        {
            get
            {
                return _Scene;
            }
            set
            {
                if (_Scene != value)
                {
                    _Scene = value;

                    RaisePropertyChanged(nameof(Scene));
                }
            }
        }

        public ObservableCollection<Character> CharactersInScene
        {
            get
            {
                return ProjectService.Instance.FindLinks<Character>(Scene);
            }
        }

        public ObservableCollection<Location> LocationsInScene
        {
            get
            {
                return ProjectService.Instance.FindLinks<Location>(Scene);
            }
        }

        private Item _SelectedItem;

        public Item SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (_SelectedItem != value)
                {
                    _SelectedItem = value;

                    RaisePropertyChanged(nameof(SelectedItem));
                    RaisePropertyChanged(nameof(ItemSelected));
                }
            }
        }

        public TextFile TextFile
        {
            get
            {
                return Scene.TextFile;
            }
        }

        public bool ItemSelected { get { return SelectedItem != null; } }

    }
}
