namespace Scribble.ViewModels.DialogViewModels
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Input;

    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;

    public class SelectProjectViewModel : DialogViewModelBase<ProjectModel>
    {
        public SelectProjectViewModel()
        {

        }

        #region Properties and Fields

        private ProjectModel _SelectedProject;

        public ProjectModel SelectedProject
        {
            get
            {
                return _SelectedProject;
            }
            set
            {
                if (_SelectedProject != value)
                {
                    _SelectedProject = value;

                    RaisePropertyChanged(nameof(SelectedProject));
                    RaisePropertyChanged(nameof(ProjectSelected));
                }
            }
        }

        public ICollection Projects
        {
            get
            {
                return ProjectService.Instance.Projects;
            }
        }

        public bool ProjectSelected { get { return SelectedProject != null; } }

        #endregion

        #region ICommands

        private ICommand _CreateCommand;

        public ICommand CreateCommand
        {
            get
            {
                return _CreateCommand ?? (_CreateCommand = new RelayCommand(() =>
                {
                    var dialog = new CreateProjectViewModel();

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        ProjectService.Instance.AddProject(result);
                    }
                }));
            }
        }

        private ICommand _RemoveProjectCommand;

        public ICommand RemoveProjectCommand
        {
            get
            {
                return _RemoveProjectCommand ?? (_RemoveProjectCommand = new RelayCommand(() => {
                    if (SelectedProject != null)

                        if (MessageBox.Show("Are you sure? Project will be deleted permanently!", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            ProjectService.Instance.Delete_Project(SelectedProject);
                }));
            }
        }

        private ICommand _SelectCommand;

        public ICommand SelectCommand
        {
            get
            {
                return _SelectCommand ?? (_SelectCommand = new RelayCommand<IDialogWindow>((window) =>
                {
                    this.CloseDialogWithResult(window, SelectedProject);
                }));
            }
        }

        #endregion
    }
}
