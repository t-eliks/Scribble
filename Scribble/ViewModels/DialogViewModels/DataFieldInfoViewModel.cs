namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class DataFieldInfoViewModel : DialogViewModelBase<DataField>
    {
        public DataFieldInfoViewModel()
        {

        }

        private BaseItem _Item;

        public BaseItem Item
        {
            get
            {
                return _Item;
            }
            set
            {
                if (_Item != value)
                {
                    _Item = value;

                    RaisePropertyChanged(nameof(Item));
                }
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

        private TreeViewItem _SelectedItem;

        public TreeViewItem SelectedItem
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

                    if (value != null)
                        Name = (string)value.Header;

                    RaisePropertyChanged(nameof(SelectedItem));
                }
            }
        }

        private ICommand _CloseCommand;

        public ICommand CloseCommand
        {
            get
            {
                return _CloseCommand ?? (_CloseCommand = new RelayCommand<IDialogWindow>((window) => {
                    CloseDialogWithResult(window, new DataField(Name));
                }));
            }
        }
    }
}
