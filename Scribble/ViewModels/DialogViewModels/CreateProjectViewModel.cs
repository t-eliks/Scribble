namespace Scribble.ViewModels.DialogViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;

    public class CreateProjectViewModel : DialogViewModelBase<ProjectModel>
    {
        public CreateProjectViewModel()
        {

        }

        private ICommand _CreateCommand;

        public ICommand CreateCommand
        {
            get
            {
                return _CreateCommand ?? (_CreateCommand = new RelayCommand<IDialogWindow>((window) =>
                {
                    this.CloseDialogWithResult(window, ProjectService.Instance.SetUpTestProject(Name, Author, SelectedForm, SelectedGenre));
                }));
            }
        }

        private string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;

                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        private string _Author;

        public string Author
        {
            get
            {
                return _Author;
            }
            set
            {
                if (_Author != value)
                {
                    _Author = value;

                    RaisePropertyChanged(nameof(Author));
                }
            }
        }

        public ObservableCollection<Genres> Genres
        {
            get
            {
                return new ObservableCollection<Genres>(Enum.GetValues(typeof(Genres)).Cast<Genres>());
            }
        }

        private Genres _SelectedGenre;

        public Genres SelectedGenre
        {
            get
            {
                return _SelectedGenre;
            }
            set
            {
                if (_SelectedGenre != value)
                {
                    _SelectedGenre = value;

                    RaisePropertyChanged(nameof(SelectedGenre));
                }
            }
        }

        public ObservableCollection<Forms> Forms
        {
            get
            {
                return new ObservableCollection<Forms>(Enum.GetValues(typeof(Forms)).Cast<Forms>());
            }
        }

        private Forms _SelectedForm;

        public Forms SelectedForm
        {
            get
            {
                return _SelectedForm;
            }
            set
            {
                if (_SelectedForm != value)
                {
                    _SelectedForm = value;

                    RaisePropertyChanged(nameof(SelectedForm));
                }
            }
        }

    }
}
