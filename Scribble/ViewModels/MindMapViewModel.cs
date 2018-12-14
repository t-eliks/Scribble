namespace Scribble.ViewModels
{
    using Scribble.Controls;
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.ViewModels.DialogViewModels;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class MindMapViewModel : BaseViewModel, IViewItemViewModel
    {
        public MindMapViewModel()
        {

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
        private ICommand _RefreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() =>
                {
                    foreach (var item in MindMap.Content)
                    {
                        //item.CallLineUpdate();
                    }
                }));
            }
        }

        private ICommand _EditInfoCommand;

        public ICommand EditInfoCommand
        {
            get
            {
                return _EditInfoCommand ?? (_EditInfoCommand = new RelayCommand(() => 
                {
                    var dialog = new MindMapInfoViewModel() { MindMap = MindMap };

                    MainViewModel._DialogService.OpenDialog(dialog);
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
                    dialog.SelectedItems = SelectedItems();
                    dialog.Title = "All characters in project";
                    dialog.Warning = "Character already in outline.";
                    dialog.Button_Text = "Add character";

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        MindMap.Content.Add(new MindMapItemModel(result));
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
                    dialog.SelectedItems = SelectedItems();
                    dialog.Title = "All locations in project";
                    dialog.Warning = "Location already in scene.";
                    dialog.Button_Text = "Add location";

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        MindMap.Content.Add(new MindMapItemModel(result));
                    }
                }));
            }
        }

        private ICommand _AddSceneCommand;

        public ICommand AddSceneCommand
        {
            get
            {
                return _AddSceneCommand ?? (_AddSceneCommand = new RelayCommand(() =>
                {
                    var dialog = new SelectViewModel();
                    dialog.Collection = new ObservableCollection<BaseItem>(ProjectService.Instance.GetItemsOfType<Scene>().Cast<BaseItem>());
                    dialog.SelectedItems = SelectedItems();
                    dialog.Title = "All scenes in project";
                    dialog.Warning = "Scene already in outline.";
                    dialog.Button_Text = "Add scene";

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        MindMap.Content.Add(new MindMapItemModel(result));
                    }
                }));
            }
        }

        private ICommand _AddMindMapStringCommand;

        public ICommand AddMindMapStringCommand
        {
            get
            {
                return _AddMindMapStringCommand ?? (_AddMindMapStringCommand = new RelayCommand(() =>
                {
                    MindMap.Content.Add(new MindMapItemModel(new MindMapString("New label", "New label content.")));
                }));
            }
        }

        private ObservableCollection<BaseItem> SelectedItems()
        {
            var items = new ObservableCollection<BaseItem>();

            foreach (var item in MindMap.Content)
            {
                if (items.Contains(item.MindMapItem))
                    items.Add(item.MindMapItem);
            }

            return items;
        }
    }
}
