namespace Scribble.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;
    using Scribble.Controls;
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Logic.Dialog;
    using Scribble.Models;
    using Scribble.ViewModels.DialogViewModels;

    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            _DialogService = new DialogService();

            if (ProjectService.Instance.ShowSelectProjectDialog() == null)
                Environment.Exit(0);
        }


        #region ICommands

        private ICommand _OpenProjectCommand;

        public ICommand OpenProjectCommand
        {
            get
            {
                return _OpenProjectCommand ?? (_OpenProjectCommand = new RelayCommand(() => 
                {
                    ProjectService.Instance.ShowSelectProjectDialog();

                    RaisePropertyChanged(nameof(TreeViewItems));
                }));
            }
        }

        private ICommand _SwitchToTimelineView;

        public ICommand SwitchToTimelineView
        {
            get
            {
                return _SwitchToTimelineView ?? (_SwitchToTimelineView = new RelayCommand(() => 
                {
                    if (SelectedProjectItem != null)
                        SelectedProjectItem.IsSelected = false;

                    ViewItemService.Instance.AddViewItem("Timeline", new TimelineViewModel() { Timelines = ProjectService.Instance.ActiveProject.Timelines });

                    RaisePropertyChanged(nameof(IsTimelineView));
                }));
            }
        }

        private ICommand _AddFileCommand;

        public ICommand AddFileCommand
        {
            get
            {
                return _AddFileCommand ?? (_AddFileCommand = new RelayCommand<ItemTypes>((type) => 
                {
                    if (SelectedProjectItem != null && SelectedProjectItem is ProjectFolder folder)
                    {
                        switch (type)
                        {
                            case ItemTypes.Character:
                                ProjectService.Instance.AddCharacter(folder);
                                break;
                            case ItemTypes.Location:
                                ProjectService.Instance.AddLocation(folder);
                                break;
                            case ItemTypes.Scene:
                                ProjectService.Instance.AddScene(folder);
                                break;
                            default:
                                break;
                        }

                        RaisePropertyChanged(nameof(TreeViewItems));
                    }
                }));
            }
        }

        private ICommand _AddFolderCommand;

        public ICommand AddFolderCommand
        {
            get
            {
                return _AddFolderCommand ?? (_AddFolderCommand = new RelayCommand(() => 
                {
                    if (SelectedProjectItem != null && SelectedProjectItem is ProjectFolder folder)
                    {
                        ProjectService.Instance.AddFolder(folder);

                        RaisePropertyChanged(nameof(TreeViewItems));
                    }
                }));
            }
        }

        private ICommand _RemoveCommand;

        public ICommand RemoveCommand
        {
            get
            {
                return _RemoveCommand ?? (_RemoveCommand = new RelayCommand(() => 
                {
                    if (SelectedProjectItem != null)
                    {
                        ProjectService.Instance.RemoveItem(_SelectedProjectItem);

                        RaisePropertyChanged(nameof(TreeViewItems));
                    }
                }));
            }
        }

        private ICommand _RenameCommand;

        public ICommand RenameCommand
        {
            get
            {
                return _RenameCommand ?? (_RenameCommand = new RelayCommand(() => 
                {
                    if (SelectedProjectItem != null)
                        Editing = true;
                }));
            }
        }

        private ICommand _SaveCommand;

        public ICommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new RelayCommand(() =>  { ProjectService.Instance.SaveActiveProject(); }));
            }
        }

        private ICommand _CollapseCommand;

        public ICommand CollapseCommand
        {
            get
            {
                return _CollapseCommand ?? (_CollapseCommand = new RelayCommand(() => 
                {
                    foreach (var item in ProjectService.Instance.ActiveProject.ProjectFiles)
                    {
                        item.Collapse();
                    }
                }));
            }
        }

        private ICommand _CloseTabCommand;

        public ICommand CloseTabCommand
        {
            get
            {
                return _CloseTabCommand ?? (_CloseTabCommand = new RelayCommand<TabControlViewItem>((tab) => 
                {
                    ViewItemService.Instance.CloseTab(tab);
                }));
            }
        }

        private ICommand _CloseOtherTabsCommand;

        public ICommand CloseOtherTabsCommand
        {
            get
            {
                return _CloseOtherTabsCommand ?? (_CloseOtherTabsCommand = new RelayCommand<TabControlViewItem>((tab) =>
                {
                    ViewItemService.Instance.CloseOtherTabs(tab);
                }));
            }
        }

        private void ChangeView()
        {
            if (SelectedProjectItem != null)
            {
                IViewItemViewModel vm = null;

                if (SelectedProjectItem is ProjectFolder f)
                {
                    if (f.RootFolder)
                        vm = new ProjectItemsOverViewModel();
                    else
                        vm = new BulletinBoardViewModel() { Folder = f };
                }
                else
                    switch (SelectedProjectItem)
                    {
                        case Character c:
                            vm = new CharacterDetailsViewModel() { Character = c };
                            break;
                        case Location l:
                            vm = new LocationDetailsViewModel() { Location = l };
                            break;
                        case Scene s:
                            vm = new SceneViewModel() { Scene = s };
                            break;
                    }

                if (vm != null)
                    ViewItemService.Instance.AddViewItem(SelectedProjectItem, vm);

                RaisePropertyChanged(nameof(IsTimelineView));
            }
        }

        private ICommand _OpenPreferencesCommand;

        public ICommand OpenPreferencesCommand
        {
            get
            {
                return _OpenPreferencesCommand ?? (_OpenPreferencesCommand = new RelayCommand(() => {
                    var dialog = new PreferencesViewModel();

                    var result = _DialogService.OpenDialog(dialog);
                }));
            }
        }

        #endregion

        #region Properties and Fields

        internal static DialogService _DialogService;

        public static RoutedEventHandler OnSceneViewChanged;

        public ObservableCollection<TabControlViewItem> ViewItems
        {
            get
            {
                return ViewItemService.Instance.ViewItems;
            }
        }

        public bool ToolButtonEnabled { get { return SelectedProjectItem != null; } }

        public string ProjectName { get { return ProjectService.Instance.ActiveProject.Name; } }

        public ObservableCollection<Item> TreeViewItems
        {
            get
            {
                return ProjectService.Instance.ActiveProject?.ProjectFiles;
            }
        }

        private BaseItem _SelectedProjectItem;

        public BaseItem SelectedProjectItem
        {
            get
            {
                return _SelectedProjectItem;
            }
            set
            {
                if (_SelectedProjectItem != value)
                {
                    _SelectedProjectItem = value;

                    if (Editing)
                        Editing = false;

                    RaisePropertyChanged(nameof(SelectedProjectItem));
                    RaisePropertyChanged(nameof(ToolButtonEnabled));

                    ChangeView();
                }

            }
        }

        private bool _Editing;

        public bool Editing
        {
            get
            {
                return _Editing;
            }
            set
            {
                if (_Editing != value)
                {
                    _Editing = value;

                    RaisePropertyChanged(nameof(Editing));
                }
            }
        }

        private string _SearchQuery;

        public string SearchQuery
        {
            get
            {
                return _SearchQuery;
            }
            set
            {
                if (_SearchQuery != value)
                {
                    _SearchQuery = value;

                    SearchResults = ProjectService.Instance.FindTags(value, 5);

                    IsSearchOpen = true;

                    RaisePropertyChanged(nameof(SearchResults));
                    RaisePropertyChanged(nameof(IsSearchOpen));
                    RaisePropertyChanged(nameof(SearchQuery));
                }
            }
        }

        private ObservableCollection<Item> _SearchResults;

        public ObservableCollection<Item> SearchResults
        {
            get
            {
                return _SearchResults ?? (_SearchResults = new ObservableCollection<Item>());
            }
            set
            {
                if (_SearchResults != value)
                {
                    _SearchResults = value;

                    RaisePropertyChanged(nameof(SearchResults));
                }
            }
        }

        public bool IsTimelineView { get { return false; } }

        public bool IsSearchOpen { get; set; }

        #endregion

        #region Methods



        #endregion
    }
}
