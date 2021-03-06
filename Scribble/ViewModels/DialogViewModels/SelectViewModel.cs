﻿namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class SelectViewModel : DialogViewModelBase<BaseItem>
    {
        public SelectViewModel()
        {

        }

        private ICommand _AddItemCommand;

        public ICommand AddItemCommand
        {
            get
            {
                return _AddItemCommand ?? (_AddItemCommand = new RelayCommand<IDialogWindow>((window) =>
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

        private ICommand _CloseCommand;

        public ICommand CloseCommand
        {
            get
            {
                return _CloseCommand ?? (_CloseCommand = new RelayCommand<IDialogWindow>((window) => { CloseDialogWithResult(window, null); }));
            }
        }

        private IEnumerable<BaseItem> _Collection;

        public IEnumerable<BaseItem> Collection
        {
            get
            {
                return _Collection ?? (_Collection = new Collection<BaseItem>());
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

        private bool _ListView = true;

        public bool ListView
        {
            get
            {
                return _ListView;
            }
            set
            {
                if (_ListView != value)
                {
                    _ListView = value;

                    if (value && GridView)
                        GridView = false;

                    RaisePropertyChanged(nameof(ListView));
                }
            }
        }

        private bool _GridView;

        public bool GridView
        {
            get
            {
                return _GridView;
            }
            set
            {
                if (_GridView != value)
                {
                    _GridView = value;

                    if (value && ListView)
                        ListView = false;

                    RaisePropertyChanged(nameof(GridView));
                }
            }
        }

        private ICollection<BaseItem> _SelectedItems;

        public ICollection<BaseItem> SelectedItems
        {
            get
            {
                return _SelectedItems ?? (_SelectedItems = new Collection<BaseItem>());
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

        private string _SearchQuery;

        public string SearchQuery
        {
            get
            {
                return _SearchQuery;
            }
            set
            {
                if (_SearchQuery != value)
                {
                    _SearchQuery = value;

                    var filtered = new ObservableCollection<BaseItem>();

                    foreach (var item in Collection)
                    {
                        if (item is ISearchable s && s.CheckMatch(value))
                            filtered.Add(item as BaseItem);
                    }

                    FilteredItems = filtered.OrderBy(x => x.Name);

                    RaisePropertyChanged(nameof(SearchQuery));
                }
            }
        }

        private IOrderedEnumerable<BaseItem> _FilteredItems;

        public IOrderedEnumerable<BaseItem> FilteredItems
        {
            get
            {
                return _FilteredItems ?? (_FilteredItems = Collection.OrderBy(x => x.Name));
            }
            set
            {
                if (_FilteredItems != value)
                {
                    _FilteredItems = value;

                    RaisePropertyChanged(nameof(FilteredItems));
                }
            }
        }
    }
}
