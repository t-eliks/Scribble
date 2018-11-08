namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Logic.Dialog;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class SelectLocationViewModel : DialogViewModelBase<Location>
    {
        public SelectLocationViewModel()
        {
            
        }

        private ICommand _AddLocationCommand;

        public ICommand AddLocationCommand
        {
            get
            {
                return _AddLocationCommand ?? (_AddLocationCommand = new RelayCommand<IDialogWindow>((window) => 
                {
                    if (SelectedLocation != null && !SelectedLocations.Contains(SelectedLocation))
                        CloseDialogWithResult(window, SelectedLocation);
                    else
                        MessageBox.Show("Location has already been added to this scene.", "Already in scene", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }));
            }
        }

        private ICommand _CloseCommand;

        public ICommand CloseCommand
        {
            get
            {
                return _CloseCommand ?? (_CloseCommand = new RelayCommand<IDialogWindow>((window) => { CloseDialogWithResult(window, null); }));
            }
        }

        private ObservableCollection<Location> _Locations;

        public ObservableCollection<Location> Locations
        {
            get
            {
                return _Locations;
            }
            set
            {
                if (_Locations != value)
                {
                    _Locations = value;

                    RaisePropertyChanged(nameof(Locations));
                }
            }
        }

        private ObservableCollection<Location> _SelectedLocations;

        public ObservableCollection<Location> SelectedLocations
        {
            get
            {
                return _SelectedLocations;
            }
            set
            {
                if (_SelectedLocations != value)
                {
                    _SelectedLocations = value;

                    RaisePropertyChanged(nameof(SelectedLocations));
                }
            }
        }

        private Location _SelectedLocation;

        public Location SelectedLocation
        {
            get
            {
                return _SelectedLocation;
            }
            set
            {
                if (_SelectedLocation != value)
                {
                    _SelectedLocation = value;

                    RaisePropertyChanged(nameof(SelectedLocation));
                    RaisePropertyChanged(nameof(CanAdd));
                }
            }
        }

        public bool CanAdd { get { return SelectedLocation != null; } }
    }
}
