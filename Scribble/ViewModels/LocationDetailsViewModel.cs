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

        #endregion

    }
}
