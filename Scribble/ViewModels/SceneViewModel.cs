namespace Scribble.ViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.ViewModels.DialogViewModels;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class SceneViewModel : ItemDetailsViewModel<Scene>, IViewItemViewModel
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
                        Item.Items.Remove(character);

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
                    dialog.SelectedItems = new ObservableCollection<object>(CharactersInScene.Cast<object>());
                    dialog.Title = "All characters in project";
                    dialog.Warning = "Character already in scene.";
                    dialog.Button_Text = "Add character";

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        Item.AddItem((Item)result);

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
                        Item.Items.Remove(location);

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
                    dialog.SelectedItems = new ObservableCollection<object>(LocationsInScene.Cast<object>());
                    dialog.Title = "All locations in project";
                    dialog.Warning = "Location already in scene.";
                    dialog.Button_Text = "Add location";

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        Item.AddItem((Item)result);

                        RaisePropertyChanged(nameof(LocationsInScene));
                    }
                }));
            }
        }

        public override ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() => { RaisePropertyChanged(nameof(CharactersInScene)); RaisePropertyChanged(nameof(LocationsInScene)); }));
            }
        }

        public ObservableCollection<Character> CharactersInScene
        {
            get
            {
                return ProjectService.Instance.FindLinks<Character>(Item);
            }
        }

        public ObservableCollection<Location> LocationsInScene
        {
            get
            {
                return ProjectService.Instance.FindLinks<Location>(Item);
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
                return ((Scene)Item).TextFile;
            }
        }

        public bool ItemSelected { get { return SelectedItem != null; } }

    }
}
