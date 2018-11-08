namespace Scribble.ViewModels
{
    using Scribble.Logic;
    using Scribble.Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class TimelineViewModel : BaseViewModel
    {
        public TimelineViewModel() { }

        private ICommand _AddTimelineCommand;

        public ICommand AddTimelineCommand
        {
            get
            {
                return _AddTimelineCommand ?? (_AddTimelineCommand = new RelayCommand(() => 
                {
                    Timelines.Add(new TimelineModel(80, 2) { OnMarkedForRemoval = (s, e) => { Timelines.Remove((TimelineModel)s); } });

                    RaisePropertyChanged(nameof(Timelines));
                }));
            }
        }

        private ObservableCollection<TimelineModel> _Timelines;

        public ObservableCollection<TimelineModel> Timelines
        {
            get
            {
                return _Timelines;
            }
            set
            {
                if (_Timelines != value)
                {
                    if (value != null)
                        foreach (var item in value)
                            item.OnMarkedForRemoval += (s, e) => { Timelines.Remove(item); };
   
                    _Timelines = value;

                    RaisePropertyChanged(nameof(Timelines));
                }
            }
        }

        public Collection<Scene> Content
        {
            get
            {
                return ProjectService.Instance.GetItemsOfType<Scene>();
            }
        }
    }
}
