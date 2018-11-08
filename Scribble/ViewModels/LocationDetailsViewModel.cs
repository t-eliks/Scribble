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
                    RaisePropertyChanged(nameof(Name));
                    RaisePropertyChanged(nameof(Description));
                    RaisePropertyChanged(nameof(Details));
                    RaisePropertyChanged(nameof(Notes));
                    RaisePropertyChanged(nameof(Tags));
                }
            }
        }

        public string Name
        {
            get
            {
                return Location.Name;
            }
            set
            {
                Location.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get
            {
                return Location.Description;
            }
            set
            {
                Location.Description = value;
            }
        }

        public string Details
        {
            get
            {
                return Location.Details;
            }
            set
            {
                Location.Details = value;
            }
        }

        public string Notes
        {
            get
            {
                return Location.Notes;
            }
            set
            {
                Location.Notes = value;
            }
        }

        public string Tags
        {
            get
            {
                return Location.Tags;
            }
            set
            {
                Location.Tags = value;
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
