namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.ViewModels;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows;
    using System.Windows.Input;

    [Serializable]
    public class MindMapModel : BaseModel, ISerializable, IViewItem
    {
        public MindMapModel()
        {

        }

        private ObservableCollection<MindMapItemModel> _Content;

        public ObservableCollection<MindMapItemModel> Content
        {
            get
            {
                return _Content ?? (_Content = new ObservableCollection<MindMapItemModel>());
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

        public event RoutedEventHandler OnHeaderChanged;

        private ICommand _OpenCommand;

        public ICommand OpenCommand
        {
            get
            {
                return _OpenCommand ?? (_OpenCommand = new RelayCommand(() => { ViewItemService.Instance.AddViewItem(this, new MindMapViewModel() { MindMap = this }); }));
            }
        }

        private string _Header = "New Mindmap";

        public string Header
        {
            get
            {
                return _Header;
            }
            set
            {
                if (_Header != value)
                {
                    _Header = value;

                    OnHeaderChanged?.Invoke(this, new RoutedEventArgs());

                    RaisePropertyChanged(nameof(Header));
                }
            }
        }

        private string _Description = "No description.";

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    _Description = value;

                    RaisePropertyChanged(nameof(Description));
                }
            }
        }

        protected MindMapModel(SerializationInfo info, StreamingContext context)
        {
            Content = (ObservableCollection<MindMapItemModel>)info.GetValue("content", typeof(ObservableCollection<MindMapItemModel>));
            Header = info.GetString("header");
            Description = info.GetString("description");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("content", Content);
            info.AddValue("header", Header);
            info.AddValue("description", Description);
        }

    }
}
