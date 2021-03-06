﻿namespace Scribble.ViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class ProjectItemsOverViewModel : BaseViewModel, IViewItemViewModel
    {
        public ProjectItemsOverViewModel()
        {
            Project = ProjectService.Instance.ActiveProject;

            Scenes = ProjectService.Instance.GetItemsOfType<Scene>();

            CurrentList = Scenes;

            _Initializing = true;

            if (Characters != null && Characters[0] != null)
                SelectedCharacter = Characters[0];

            if (Locations != null && Locations[0] != null)
                SelectedLocation = Locations[0];

            ApplyFilter();

            _Initializing = false;
        }

        private bool _Initializing;

        private ICommand _RefreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() => { RaisePropertyChanged(nameof(Scenes)); RaisePropertyChanged(nameof(NumberOfCharacters));
                    RaisePropertyChanged(nameof(NumberOfLocations)); RaisePropertyChanged(nameof(NumberOfScenes));
                }));
            }
        }

        private ProjectModel _Project;

        public ProjectModel Project
        {
            get
            {
                return _Project;
            }
            set
            {
                if (_Project != value)
                {
                    _Project = value;

                    RaisePropertyChanged(nameof(Project));
                    RaisePropertyChanged(nameof(NumberOfCharacters));
                    RaisePropertyChanged(nameof(NumberOfLocations));
                    RaisePropertyChanged(nameof(NumberOfScenes));
                }
            }
        }

        public int NumberOfScenes { get { return ProjectService.Instance.GetItemsOfType<Scene>().Count; } }

        public int NumberOfCharacters { get { return ProjectService.Instance.GetItemsOfType<Character>().Count; } }

        public int NumberOfLocations { get { return ProjectService.Instance.GetItemsOfType<Location>().Count; } }

        private ObservableCollection<Scene> _Scenes;

        public ObservableCollection<Scene> Scenes
        {
            get
            {
                return _Scenes;
            }
            set
            {
                if (_Scenes != value)
                {
                    _Scenes = value;

                    RaisePropertyChanged(nameof(Scenes));
                }
            }
        }

        private ObservableCollection<Scene> _CurrentList;

        public ObservableCollection<Scene> CurrentList
        {
            get
            {
                return _CurrentList;
            }
            set
            {
                if (_CurrentList != value)
                {
                    _CurrentList = value;

                    RaisePropertyChanged(nameof(CurrentList));
                }
            }
        }

        private Character _SelectedCharacter;

        public Character SelectedCharacter
        {
            get
            {
                return _SelectedCharacter;
            }
            set
            {
                if (_SelectedCharacter != value)
                {
                    _SelectedCharacter = value;

                    if (!_Initializing)
                        CurrentList = ApplyFilter();

                    RaisePropertyChanged(nameof(CurrentList));
                }
            }
        }

        public ObservableCollection<Character> Characters
        {
            get
            {
                var result = new ObservableCollection<Character>();

                result.Add(new Character("Any characters", IconHelper.FindIconInResources("Character")));

                foreach (var character in ProjectService.Instance.GetItemsOfType<Character>())
                {
                    result.Add(character);
                }

                return result;
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

                    if (!_Initializing)
                        CurrentList = ApplyFilter();

                    RaisePropertyChanged(nameof(CurrentList));
                }
            }
        }

        public ObservableCollection<Location> Locations
        {
            get
            {
                var result = new ObservableCollection<Location>();

                result.Add(new Location("Any locations", IconHelper.FindIconInResources("Map")));

                foreach (var location in ProjectService.Instance.GetItemsOfType<Location>())
                {
                    result.Add(location);
                }

                return result;
            }
        }

        public ObservableCollection<Scene> ApplyFilter()
        {
            var result = Scenes.ToList();

            if (SelectedCharacter != null && !SelectedCharacter.Name.Equals("Any characters"))
                result = Scenes.Where(x => x.Items.Contains(SelectedCharacter)).ToList();

            if (SelectedLocation != null && !SelectedLocation.Name.Equals("Any locations"))
                result = result.Where(x => x.Items.Contains(SelectedLocation)).ToList();

            return new ObservableCollection<Scene>(result);
        }
    }
}