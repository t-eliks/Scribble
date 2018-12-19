namespace Scribble.ViewModels
{
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.ViewModels.DialogViewModels;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class ItemDetailsViewModel<T> : BaseViewModel where T : Item
    {
        public ItemDetailsViewModel()
        {

        }

        protected Item _Item;

        public virtual Item Item
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

        private ICommand _AddNoteCommand;

        public ICommand AddNoteCommand
        {
            get
            {
                return _AddNoteCommand ?? (_AddNoteCommand = new RelayCommand(() =>
                {
                    if (Item != null)
                    {
                        var note = new Note(Item, "New note", "No description.");
                        note.OnOpened += (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };
                        note.OnMarkedForRemoval += (o, a) => { if (Notes.Contains(note)) { Item.Notes.Remove(note); note.Delete(); } };
                        Item.Notes.Add(note);

                        ProjectService.Instance.SaveActiveProject();
                    }
                }));
            }
        }

        private ICommand _AddDataFieldCommand;

        public ICommand AddDataFieldCommand
        {
            get
            {
                return _AddDataFieldCommand ?? (_AddDataFieldCommand = new RelayCommand(() => {

                    var dialog = new DataFieldInfoViewModel() { Item = Item };

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                        DataFields.Add(result);
                }));
            }
        }

        private ICommand _EditDataFieldCommand;

        public ICommand EditDataFieldCommand
        {
            get
            {
                return _EditDataFieldCommand ?? (_EditDataFieldCommand = new RelayCommand<DataField>((datafield) => 
                {
                    var dialog = new DataFieldInfoViewModel() { Item = Item, Name = datafield.Name };

                    var result = MainViewModel._DialogService.OpenDialog(dialog);

                    if (result != null)
                    {
                        datafield.Name = result.Name;
                    }
                }));
            }
        }

        private ICommand _RemoveDataFieldCommand;

        public ICommand RemoveDataFieldCommand
        {
            get
            {
                return _RemoveDataFieldCommand ?? (_RemoveDataFieldCommand = new RelayCommand<DataField>((datafield) =>
                {
                    if (Item.DataFields.Contains(datafield))
                        Item.DataFields.Remove(datafield);
                }));
            }
        }

        public ObservableCollection<Note> Notes
        {
            get
            {
                foreach (var note in Item.Notes)
                {
                    //Prevent multiple subscriptions
                    note.OnOpened -= (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };
                    note.OnOpened += (s, e) => { ViewItemService.Instance.AddViewItem(note, new NoteViewModel() { Note = note }); };

                    note.OnMarkedForRemoval -= (o, a) => { if (Notes.Contains(note)) { Notes.Remove(note); note.Delete(); } };
                    note.OnMarkedForRemoval += (o, a) => { if (Notes.Contains(note)) { Notes.Remove(note); note.Delete(); } };
                }

                return Item.Notes;
            }
        }

        public ObservableCollection<Scene> Scenes
        {
            get
            {
                return ProjectService.Instance.FindBiLinks<Scene>(Item);
            }
        }

        protected ICommand _RefreshCommand;

        public virtual ICommand RefreshCommand
        {
            get
            {
                return _RefreshCommand ?? (_RefreshCommand = new RelayCommand(() => { RaisePropertyChanged(nameof(Scenes)); }));
            }
        }


        public ObservableCollection<DataField> DataFields
        {
            get
            {
                return Item.DataFields;
            }
        }
    }
}
