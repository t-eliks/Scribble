namespace Scribble.ViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class TimelineMapViewModel : BaseViewModel, IViewItemViewModel
    {
        public TimelineMapViewModel()
        {

        }

        private TimelineMapModel _Timeline;

        public TimelineMapModel Timeline
        {
            get
            {
                return _Timeline;
            }
            set
            {
                if (_Timeline != value)
                {
                    _Timeline = value;

                    RaisePropertyChanged(nameof(Timeline));
                }
            }
        }

        public ObservableCollection<TimelineModel> Content
        {
            get
            {
                return Timeline.MapContent;
            }
        }

        private ICommand _AddRowCommand;

        public ICommand AddRowCommand
        {
            get
            {
                return _AddRowCommand ?? (_AddRowCommand = new RelayCommand<TimelineModel>((t) => { if (t != null) t.Rows += 1; }));
            }
        }

        private ICommand _SubtractRowCommand;

        public ICommand SubtractRowCommand
        {
            get
            {
                return _SubtractRowCommand ?? (_SubtractRowCommand = new RelayCommand<TimelineModel>((t) => { if (t != null && t.Rows > 1) t.Rows -= 1; }));
            }
        }

        private ICommand _RefreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() =>
                {
                    RaisePropertyChanged(nameof(Content));
                }));
            }
        }

        private ICommand _AddSceneCommand;

        public ICommand AddSceneCommand
        {
            get
            {
                return _AddSceneCommand ?? (_AddSceneCommand = new RelayCommand<TimelineModel>((t) =>
                {
                    var result = SelectionService.SelectItems(GetSelectedItems<Scene>(t), "All scenes in project", "Scene already in mindmap.");

                    if (result != null)
                    {
                        t.AddContent(new CanvasItemModel(result));
                    }
                }));
            }
        }

        private ICommand _AddTimelineCommand;

        public ICommand AddTimelineCommand
        {
            get
            {
                return _AddTimelineCommand ?? (_AddTimelineCommand = new RelayCommand(() => {
                    var timeline = new TimelineModel();
                    ProjectService.Instance.AddSymbioticLink(new SymbioticLink<TimelineMapModel, TimelineModel>(Timeline, timeline));
                    RaisePropertyChanged(nameof(Content));
                }));
            }
        }

        private ObservableCollection<T> GetSelectedItems<T>(TimelineModel timeline) where T : BaseItem
        {
            var items = new ObservableCollection<T>();

            foreach (var item in timeline.Content)
            {
                if (item.CanvasItem is T t && !items.Contains(t))
                    items.Add(t);
            }

            return items;
        }
    }
}
