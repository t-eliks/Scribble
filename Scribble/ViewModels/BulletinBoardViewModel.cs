namespace Scribble.ViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.ViewModels.DialogViewModels;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class BulletinBoardViewModel : BaseViewModel, IViewItemViewModel
    {
        public BulletinBoardViewModel()
        {

        }

        private ICommand _RefreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() => { RaisePropertyChanged(nameof(Items)); }));
            }
        }

        private ICommand _ToggleIsSelectedCommand;

        public ICommand ToggleIsSelectedCommand
        {
            get
            {
                return _ToggleIsSelectedCommand ?? (_ToggleIsSelectedCommand = new RelayCommand<BaseItem>((item) => { ViewItemService.Instance.AddViewItem(item); }));
            }
        }

        private ProjectFolder _Folder;

        public ProjectFolder Folder
        {
            get
            {
                return _Folder;
            }
            set
            {
                if (_Folder != value)
                {
                    _Folder = value;

                    RaisePropertyChanged(nameof(Folder));
                }
            }
        }

        public ObservableCollection<BaseItem> Items
        {
            get
            {
                return Folder?.Content;
            }
        }

        private ICommand _EditNoteCommand;

        public ICommand EditNoteCommand
        {
            get
            {
                return _EditNoteCommand ?? (_EditNoteCommand = new RelayCommand<Note>((note) =>
                {
                    var dialog = new TwoFieldInfoViewModel(false) { Item = note };

                    MainViewModel._DialogService.OpenDialog(dialog);
                }));
            }
        }

        private ICommand _RemoveNoteCommand;

        public ICommand RemoveNoteCommand
        {
            get
            {
                return _RemoveNoteCommand ?? (_RemoveNoteCommand = new RelayCommand<Note>((note) =>
                {
                    if (MessageBox.Show("Are you sure? This cannot be undone.", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        if (Items.Contains(note))
                            Folder.DeleteItem(note);

                        note.Delete();

                        RaisePropertyChanged(nameof(Items));
                    }
                }));
            }
        }
    }
}
