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
    using System.Windows.Media;

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

        public ObservableCollection<MindMapItemModel> Content
        {
            get
            {
                return MindMap.MapContent;
            }
        }

        private ICommand _RefreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() =>
                {
                    RaisePropertyChanged(nameof(Content));
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
                    var dialog = new TwoFieldInfoViewModel(false) { Item = MindMap };

                    MainViewModel._DialogService.OpenDialog(dialog);
                }));
            }
        }

        private ICommand _EndCustomizeCommand;

        public ICommand EndCustomizeCommand
        {
            get
            {
                return _EndCustomizeCommand ?? (_EndCustomizeCommand = new RelayCommand(() =>
                {
                    CustomizeMode = false;
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
                    dialog.SelectedItems = GetSelectedItems();
                    dialog.Title = "All characters in project";
                    dialog.Warning = "Character already in mindmap.";
                    dialog.Button_Text = "Add character";

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        ProjectService.Instance.AddSymbioticLink(new SymbioticLink<MindMapModel, MindMapItemModel>(MindMap, new MindMapItemModel((IMindMapItem)result)));

                        RaisePropertyChanged(nameof(Content));
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
                    dialog.SelectedItems = GetSelectedItems();
                    dialog.Title = "All locations in project";
                    dialog.Warning = "Location already in mindmap.";
                    dialog.Button_Text = "Add location";

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        ProjectService.Instance.AddSymbioticLink(new SymbioticLink<MindMapModel, MindMapItemModel>(MindMap, new MindMapItemModel((IMindMapItem)result)));

                        RaisePropertyChanged(nameof(Content));
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
                    dialog.SelectedItems = GetSelectedItems();
                    dialog.Title = "All scenes in project";
                    dialog.Warning = "Scene already in mindmap.";
                    dialog.Button_Text = "Add scene";

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        ProjectService.Instance.AddSymbioticLink(new SymbioticLink<MindMapModel, MindMapItemModel>(MindMap, new MindMapItemModel((IMindMapItem)result)));

                        RaisePropertyChanged(nameof(Content));
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
                    MindMap.MapContent.Add(new MindMapItemModel(new MindMapString("New label", "New label content.")));

                    ProjectService.Instance.AddSymbioticLink(new SymbioticLink<MindMapModel, MindMapItemModel>(MindMap, new MindMapItemModel(new MindMapString("New label", "New label content."))));

                    RaisePropertyChanged(nameof(Content));
                }));
            }
        }

        private MindMapItemModel _SelectedItem;

        public MindMapItemModel SelectedItem
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

                    foreach (var item in App.Current.FindResource("MindMapColors") as HeaderColor[])
                    {
                        if (item.Brush.ToString() == value.BackgroundColor)
                            SelectedBackgroundColor = item;
                        if (item.Brush.ToString() == value.ForegroundColor)
                            SelectedForegroundColor = item;
                    }

                    HeaderFontSize = value.HeaderFontSize;
                    ContentFontSize = value.ContentFontSize;

                    HeaderBold = value.HeaderBold;
                    ContentBold = value.ContentBold;

                    RaisePropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private bool _CustomizeMode;

        public bool CustomizeMode
        {
            get
            {
                return _CustomizeMode;
            }
            set
            {
                if (_CustomizeMode != value)
                {
                    _CustomizeMode = value;

                    RaisePropertyChanged(nameof(CustomizeMode));
                }
            }
        }

        private HeaderColor _SelectedBackgroundColor;

        public HeaderColor SelectedBackgroundColor
        {
            get
            {
                return _SelectedBackgroundColor;
            }
            set
            {
                if (_SelectedBackgroundColor != value)
                {
                    _SelectedBackgroundColor = value;

                    if (SelectedItem != null)
                        SelectedItem.BackgroundColor = value.Brush.ToString();

                    RaisePropertyChanged(nameof(SelectedBackgroundColor));
                }
            }
        }

        private HeaderColor _SelectedForegroundColor;

        public HeaderColor SelectedForegroundColor
        {
            get
            {
                return _SelectedForegroundColor;
            }
            set
            {
                if (_SelectedForegroundColor != value)
                {
                    _SelectedForegroundColor = value;

                    if (SelectedItem != null)
                        SelectedItem.ForegroundColor = value.Brush.ToString();

                    RaisePropertyChanged(nameof(SelectedForegroundColor));
                }
            }
        }

        public int[] FontSizes { get; } = { 8, 9, 10, 11, 12, 13, 14, 16, 18, 20, 22, 24, 28, 32, 36, 44, 52, 60, 72, 84, 96 };

        private int _HeaderFontSize;

        public int HeaderFontSize
        {
            get
            {
                return _HeaderFontSize;
            }
            set
            {
                if (_HeaderFontSize != value)
                {
                    _HeaderFontSize = value;

                    if (SelectedItem != null)
                        SelectedItem.HeaderFontSize = value;

                    RaisePropertyChanged(nameof(HeaderFontSize));
                }
            }
        }

        private int _ContentFontSize;

        public int ContentFontSize
        {
            get
            {
                return _ContentFontSize;
            }
            set
            {
                if (_ContentFontSize != value)
                {
                    _ContentFontSize = value;

                    if (SelectedItem != null)
                        SelectedItem.ContentFontSize = value;

                    RaisePropertyChanged(nameof(ContentFontSize));
                }
            }
        }

        private bool _HeaderBold;

        public bool HeaderBold
        {
            get
            {
                return _HeaderBold;
            }
            set
            {
                if (_HeaderBold != value)
                {
                    _HeaderBold = value;

                    if (SelectedItem != null)
                        SelectedItem.HeaderBold = value;

                    RaisePropertyChanged(nameof(HeaderBold));
                }
            }
        }

        private bool _ContentBold;

        public bool ContentBold
        {
            get
            {
                return _ContentBold;
            }
            set
            {
                if (_ContentBold != value)
                {
                    _ContentBold = value;

                    if (SelectedItem != null)
                        SelectedItem.ContentBold = value;

                    RaisePropertyChanged(nameof(ContentBold));
                }
            }
        }

        private ObservableCollection<object> GetSelectedItems()
        {
            var items = new ObservableCollection<object>();

            foreach (var item in MindMap.MapContent)
            {
                if (!items.Contains(item.MindMapItem))
                    items.Add(item.MindMapItem);
            }

            return items;
        }
    }
}
