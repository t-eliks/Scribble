namespace Scribble.Models
{
    using Scribble.Logic;
    using Scribble.ViewModels;
    using Scribble.ViewModels.DialogViewModels;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows;
    using System.Windows.Input;

    [Serializable]
    public class TimelineModel : BaseViewModel, ISerializable
    {
        public TimelineModel(int itemheight, int rows)
        {
            Content = new ObservableCollection<Scene>();

            ItemHeight = itemheight;

            Rows = rows;
        }

        private ICommand _AddRowCommand;

        public ICommand AddRowCommand
        {
            get
            {
                return _AddRowCommand ?? (_AddRowCommand = new RelayCommand(() => { this.Rows += 1; }));
            }
        }

        private ICommand _SubtractRowCommand;

        public ICommand SubtractRowCommand
        {
            get
            {
                return _SubtractRowCommand ?? (_SubtractRowCommand = new RelayCommand(() => { this.Rows -= 1; }));
            }
        }

        private ICommand _AddSceneCommand;

        public ICommand AddSceneCommand
        {
            get
            {
                return _AddSceneCommand ?? (_AddSceneCommand = new RelayCommand(() => 
                {
                    var dialog = new SelectSceneViewModel();
                    dialog.Scenes = ProjectService.Instance.GetItemsOfType<Scene>();
                    dialog.SelectedScenes = new ObservableCollection<Scene>(Content);
                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        if (!result.IsInTimeline)
                        {
                            result.CanvasLeft = 0;
                            result.CanvasTop = 0;

                            Content.Add(result);

                            RaisePropertyChanged(nameof(Content));
                        }
                        else
                            MessageBox.Show("Scene already in a timeline", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }));
            }
        }

        private ICommand _MarkForRemovalCommand;

        public ICommand MarkForRemovalCommand
        {
            get
            {
                return _MarkForRemovalCommand ?? (_MarkForRemovalCommand = new RelayCommand(() => { OnMarkedForRemoval?.Invoke(this, new RoutedEventArgs()); }));
            }
        }

        public RoutedEventHandler OnMarkedForRemoval;

        private string _Name = "New timeline";

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

        private int _ItemHeight;

        public int ItemHeight
        {
            get
            {
                return _ItemHeight;
            }
            set
            {
                if (_ItemHeight != value)
                {
                    _ItemHeight = value;

                    RaisePropertyChanged(nameof(ItemHeight));
                }
            }
        }

        private int _Rows;

        public int Rows
        {
            get
            {
                return _Rows;
            }
            set
            {
                if (_Rows != value)
                {
                    _Rows = value;

                    RaisePropertyChanged(nameof(Rows));
                }
            }
        }

        private int _TimelineWidth = 1000;

        public int TimelineWidth
        {
            get
            {
                return _TimelineWidth;
            }
            set
            {
                if (_TimelineWidth != value)
                {
                    _TimelineWidth = value;

                    RaisePropertyChanged(nameof(TimelineWidth));
                }
            }
        }

        private ObservableCollection<Scene> _Content;

        public ObservableCollection<Scene> Content
        {
            get
            {
                return _Content;
            }
            set
            {
                if (_Content != value)
                {
                    _Content = value;

                    RaisePropertyChanged(nameof(Content));
                }
            }
        }

        #region Serialization

        protected TimelineModel(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
            Content = (ObservableCollection<Scene>)info.GetValue("content", typeof(ObservableCollection<Scene>));
            ItemHeight = info.GetInt32("itemheight");
            Rows = info.GetInt32("rows");
            TimelineWidth = info.GetInt32("width");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("content", Content);
            info.AddValue("itemheight", ItemHeight);
            info.AddValue("rows", Rows);
            info.AddValue("width", TimelineWidth);
        }

        #endregion
    }
}
