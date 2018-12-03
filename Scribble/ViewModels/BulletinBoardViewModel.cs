namespace Scribble.ViewModels
{
    using Scribble.Models;
    using System.Collections.ObjectModel;

    public class BulletinBoardViewModel : BaseViewModel
    {
        public BulletinBoardViewModel()
        {

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
