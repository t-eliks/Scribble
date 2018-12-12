namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class SelectViewModel : DialogViewModelBase<BaseItem>
    {
        public SelectViewModel()
        {

        }

        private ICommand _AddLocationCommand;

        public ICommand AddLocationCommand
        {
            get
            {
                return _AddLocationCommand ?? (_AddLocationCommand = new RelayCommand<IDialogWindow>((window) =>
                {
                    if (SelectedItem != null && !SelectedItems.Contains(SelectedItem))
                        CloseDialogWithResult(window, SelectedItem);
                    else
                        MessageBox.Show(Warning ?? "Error", "Already in scene", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }));
            }
        }

        private string _Title;

        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_Title != value)
                {
                    _Title = value;

                    RaisePropertyChanged(nameof(Title));
                }
            }
        }

        private string _Warning;

        public string Warning
        {
            get
            {
                return _Warning;
            }
            set
            {
                if (_Warning != value)
                {
                    _Warning = value;

                    RaisePropertyChanged(nameof(Warning));
                }
            }
        }

        private string _Button_Text;

        public string Button_Text
        {
            get
            {
                return _Button_Text;
            }
            set
            {
                if (_Button_Text != value)
                {
                    _Button_Text = value;

                    RaisePropertyChanged(nameof(Button_Text));
                }
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

        private ObservableCollection<BaseItem> _Collection;

        public ObservableCollection<BaseItem> Collection
        {
            get
            {
                return _Collection ?? (_Collection = new ObservableCollection<BaseItem>());
            }
            set
            {
                if (_Collection != value)
                {
                    _Collection = value;

                    RaisePropertyChanged(nameof(Collection));
                }
            }
        }

        private ObservableCollection<BaseItem> _SelectedItems;

        public ObservableCollection<BaseItem> SelectedItems
        {
            get
            {
                return _SelectedItems;
            }
            set
            {
                if (_SelectedItems != value)
                {
                    _SelectedItems = value;

                    RaisePropertyChanged(nameof(SelectedItems));
                }
            }
        }

        private BaseItem _SelectedItem;

        public BaseItem SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (_SelectedItem != value)
                {
                    _SelectedItem = value;

                    RaisePropertyChanged(nameof(SelectedItem));
                    RaisePropertyChanged(nameof(CanAdd));
                }
            }
        }

        public bool CanAdd { get { return SelectedItem != null; } }
    }
}
