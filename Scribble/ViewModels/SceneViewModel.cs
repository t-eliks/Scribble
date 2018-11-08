namespace Scribble.ViewModels
{
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.ViewModels.DialogViewModels;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class SceneViewModel : BaseViewModel
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
                    var dialog = new SelectCharacterViewModel();
                    dialog.Characters = ProjectService.Instance.GetItemsOfType<Character>();
                    dialog.SelectedCharacters = CharactersInScene;
                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        Scene.AddItem(result);

                        RaisePropertyChanged(nameof(CharactersInScene));
                    }
                } ));
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
                    var dialog = new SelectLocationViewModel();
                    dialog.Locations = ProjectService.Instance.GetItemsOfType<Location>();
                    dialog.SelectedLocations = LocationsInScene;
                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        Scene.AddItem(result);

                        RaisePropertyChanged(nameof(LocationsInScene));
                    }
                }));
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
                    RaisePropertyChanged(nameof(Name));
                    RaisePropertyChanged(nameof(Role));
                    RaisePropertyChanged(nameof(Description));
                    RaisePropertyChanged(nameof(Sights));
                    RaisePropertyChanged(nameof(Sounds));
                    RaisePropertyChanged(nameof(Smells));
                    RaisePropertyChanged(nameof(Tags));
                    RaisePropertyChanged(nameof(Outcome));
                    RaisePropertyChanged(nameof(Notes));
                }
            }
        }

        public string Name
        {
            get
            {
                return Scene.Name;
            }
            set
            {
                Scene.Name = value;

                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Role
        {
            get
            {
                return Scene.Role;
            }
            set
            {
                Scene.Role = value;
            }
        }

        public string Description
        {
            get
            {
                return Scene.Description;
            }
            set
            {
                Scene.Description = value;
            }
        }

        public string Sights
        {
            get
            {
                return Scene.Sights;
            }
            set
            {
                Scene.Sights = value;
            }
        }

        public string Sounds
        {
            get
            {
                return Scene.Sounds;
            }
            set
            {
                Scene.Sounds = value;
            }
        }

        public string Smells
        {
            get
            {
                return Scene.Smells;
            }
            set
            {
                Scene.Smells = value;
            }
        }

        public string Notes
        {
            get
            {
                return Scene.Notes;
            }
            set
            {
                Scene.Notes = value;
            }
        }

        public string Tags
        {
            get
            {
                return Scene.Tags;
            }
            set
            {
                Scene.Tags = value;
            }
        }

        public string Outcome
        {
            get
            {
                return Scene.Outcome;
            }
            set
            {
                Scene.Outcome = value;
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

        public string TextFile
        {
            get
            {
                return Scene.FilePath;
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

        public bool ItemSelected { get { return SelectedItem != null; } }
    }
}
