namespace Scribble.ViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using System.Collections.ObjectModel;
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

        public ObservableCollection<Item> Items
        {
            get
            {
                return Folder?.Items;
            }
        }
    }
}
