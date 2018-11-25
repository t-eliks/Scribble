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
