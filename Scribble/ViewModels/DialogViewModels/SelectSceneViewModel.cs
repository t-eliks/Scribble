namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class SelectSceneViewModel : DialogViewModelBase<Scene>
    {
        public SelectSceneViewModel()
        {
            
        }

        private ICommand _AddSceneCommand;

        public ICommand AddSceneCommand
        {
            get
            {
                return _AddSceneCommand ?? (_AddSceneCommand = new RelayCommand<IDialogWindow>((window) => 
                {
                    if (SelectedScene != null && !SelectedScenes.Contains(SelectedScene))
                        CloseDialogWithResult(window, SelectedScene);
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

        private ObservableCollection<Scene> _SelectedScenes;

        public ObservableCollection<Scene> SelectedScenes
        {
            get
            {
                return _SelectedScenes ?? (_SelectedScenes = new ObservableCollection<Scene>());
            }
            set
            {
                if (_SelectedScenes != value)
                {
                    _SelectedScenes = value;

                    RaisePropertyChanged(nameof(SelectedScenes));
                }
            }
        }

        private Scene _SelectedScene;

        public Scene SelectedScene
        {
            get
            {
                return _SelectedScene;
            }
            set
            {
                if (_SelectedScene != value)
                {
                    _SelectedScene = value;

                    RaisePropertyChanged(nameof(SelectedScene));
                    RaisePropertyChanged(nameof(CanAdd));
                }
            }
        }

        public bool CanAdd { get { return SelectedScene != null; } }
    }
}
