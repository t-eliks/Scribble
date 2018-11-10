namespace Scribble.ViewModels
{
    using Scribble.Models;
    using Scribble.Logic;
    using System.Collections.ObjectModel;

    public class LocationDetailsViewModel : BaseViewModel
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
