namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class SelectCharacterViewModel : DialogViewModelBase<Character>
    {
        public SelectCharacterViewModel()
        {
            
        }

        private ICommand _AddCharacterCommand;

        public ICommand AddCharacterCommand
        {
            get
            {
                return _AddCharacterCommand ?? (_AddCharacterCommand = new RelayCommand<IDialogWindow>((window) => 
                {
                    if (SelectedCharacter != null && !SelectedCharacters.Contains(SelectedCharacter))
                        CloseDialogWithResult(window, SelectedCharacter);
                    else
                        MessageBox.Show("Character already in scene.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private ObservableCollection<Character> _Characters;

        public ObservableCollection<Character> Characters
        {
            get
            {
                return _Characters;
            }
            set
            {
                if (_Characters != value)
                {
                    _Characters = value;

                    RaisePropertyChanged(nameof(Characters));
                }
            }
        }

        private ObservableCollection<Character> _SelectedCharacters;

        public ObservableCollection<Character> SelectedCharacters
        {
            get
            {
                return _SelectedCharacters;
            }
            set
            {
                if (_SelectedCharacters != value)
                {
                    _SelectedCharacters = value;

                    RaisePropertyChanged(nameof(SelectedCharacters));
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

                    RaisePropertyChanged(nameof(SelectedCharacter));
                    RaisePropertyChanged(nameof(CanAdd));
                }
            }
        }

        public bool CanAdd { get { return SelectedCharacter != null; } }
    }
}
