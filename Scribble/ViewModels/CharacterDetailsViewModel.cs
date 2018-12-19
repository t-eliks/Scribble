namespace Scribble.ViewModels
{
    using Scribble.Models;
    using Scribble.Logic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Scribble.Interfaces;

    public class CharacterDetailsViewModel : ItemDetailsViewModel<Character>, IViewItemViewModel
    {
        public CharacterDetailsViewModel()
        {

        }

        #region Properties and Fields

        public override Item Item
        {
            get
            {
                return _Item;
            }
            set
            {
                if (_Item != value)
                {
                    _Item = value;

                    RaisePropertyChanged(nameof(Item));
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
                return ((Character)Item).CharacterType == CharacterTypes.Major;
            }
            set
            {
                if (((Character)Item).CharacterType != CharacterTypes.Major)
                    ((Character)Item).CharacterType = CharacterTypes.Major;

                RaisePropertyChanged(nameof(Major));
                RaisePropertyChanged(nameof(Minor));
                RaisePropertyChanged(nameof(Background));
            }
        }

        public bool Minor
        {
            get
            {
                return ((Character)Item).CharacterType == CharacterTypes.Minor;
            }
            set
            {
                if (((Character)Item).CharacterType != CharacterTypes.Minor)
                    ((Character)Item).CharacterType = CharacterTypes.Minor;

                RaisePropertyChanged(nameof(Major));
                RaisePropertyChanged(nameof(Minor));
                RaisePropertyChanged(nameof(Background));
            }
        }

        public bool Background
        {
            get
            {
                return ((Character)Item).CharacterType == CharacterTypes.Background;
            }
            set
            {
                if (((Character)Item).CharacterType != CharacterTypes.Background)
                    ((Character)Item).CharacterType = CharacterTypes.Background;

                RaisePropertyChanged(nameof(Major));
                RaisePropertyChanged(nameof(Minor));
                RaisePropertyChanged(nameof(Background));
            }
        }

        #endregion
    }
}
