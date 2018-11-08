namespace Scribble.ViewModels
{
    using Scribble.Models;
    using Scribble.Logic;
    using System.Collections.ObjectModel;

    public class CharacterDetailsViewModel : BaseViewModel
    {
        public CharacterDetailsViewModel()
        {

        }

        #region Properties and Fields

        private Character _Character;

        public Character Character
        {
            get
            {
                return _Character;
            }
            set
            {
                if (_Character != value)
                {
                    _Character = value;

                    RaisePropertyChanged(nameof(Character));
                    RaisePropertyChanged(nameof(Major));
                    RaisePropertyChanged(nameof(Minor));
                    RaisePropertyChanged(nameof(Background));
                    RaisePropertyChanged(nameof(Full_Name));
                    RaisePropertyChanged(nameof(Short_Name));
                    RaisePropertyChanged(nameof(Description));
                    RaisePropertyChanged(nameof(Biography));
                    RaisePropertyChanged(nameof(Notes));
                    RaisePropertyChanged(nameof(Tags));
                    RaisePropertyChanged(nameof(Goals));
                }
            }
        }

        public bool Major
        {
            get
            {
                return Character.CharacterType == CharacterTypes.Major;
            }
            set
            {
                if (Character.CharacterType != CharacterTypes.Major)
                    Character.CharacterType = CharacterTypes.Major;

                RaisePropertyChanged(nameof(Major));
                RaisePropertyChanged(nameof(Minor));
                RaisePropertyChanged(nameof(Background));
            }
        }

        public bool Minor
        {
            get
            {
                return Character.CharacterType == CharacterTypes.Minor;
            }
            set
            {
                if (Character.CharacterType != CharacterTypes.Minor)
                    Character.CharacterType = CharacterTypes.Minor;

                RaisePropertyChanged(nameof(Major));
                RaisePropertyChanged(nameof(Minor));
                RaisePropertyChanged(nameof(Background));
            }
        }

        public bool Background
        {
            get
            {
                return Character.CharacterType == CharacterTypes.Background;
            }
            set
            {
                if (Character.CharacterType != CharacterTypes.Background)
                    Character.CharacterType = CharacterTypes.Background;

                RaisePropertyChanged(nameof(Major));
                RaisePropertyChanged(nameof(Minor));
                RaisePropertyChanged(nameof(Background));
            }
        }

        public string Full_Name
        {
            get
            {
                return Character.Full_Name;
            }
            set
            {
                Character.Full_Name = value;
                RaisePropertyChanged(nameof(Full_Name));
            }
        }

        public string Short_Name
        {
            get
            {
                return Character.Short_Name;
            }
            set
            {
                Character.Short_Name = value;
            }
        }

        public string Description
        {
            get
            {
                return Character.Description;
            }
            set
            {
                Character.Description = value;
            }
        }

        public string Biography
        {
            get
            {
                return Character.Biography;
            }
            set
            {
                Character.Biography = value;
            }
        }

        public string Notes
        {
            get
            {
                return Character.Notes;
            }
            set
            {
                Character.Notes = value;
            }
        }

        public string Tags
        {
            get
            {
                return Character.Tags;
            }
            set
            {
                Character.Tags = value;
            }
        }

        public string Goals
        {
            get
            {
                return Character.Goals;
            }
            set
            {
                Character.Goals = value;
            }
        }

        public ObservableCollection<Scene> Scenes
        {
            get
            {
                return ProjectService.Instance.FindBiLinks<Scene>(Character);
            }
        }

        #endregion
    }
}
