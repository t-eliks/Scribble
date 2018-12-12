namespace Scribble.ViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class MindMapSelectionViewModel : BaseViewModel, IViewItemViewModel
    {
        public MindMapSelectionViewModel()
        {

        }

        private ICommand _AddMindMapCommand;

        public ICommand AddMindMapCommand
        {
            get
            {
                return _AddMindMapCommand ?? (_AddMindMapCommand = new RelayCommand(() => { ProjectService.Instance.ActiveProject.MindMaps.Add(new MindMapModel()); }));
            }
        }

        public ObservableCollection<MindMapModel> MindMaps
        {
            get
            {
                return ProjectService.Instance.ActiveProject.MindMaps;
            }
        }

        private ICommand _RefreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() => { RaisePropertyChanged(nameof(MindMaps)); }));
            }
        }
    }
}
