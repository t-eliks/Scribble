﻿namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.Views.DialogViewModels;
    using System;
    using System.Windows.Input;

    public class TwoFieldInfoViewModel : DialogViewModelBase<ITwoField>
    {
        public TwoFieldInfoViewModel(bool nameOptional)
        {
            NameOptional = nameOptional;
        }

        public bool NameOptional { get; private set; }

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
                    if (NameOptional && String.IsNullOrWhiteSpace(value))
                        _Name = ""; //If field is equal to "", visibility is set to collapsed. For some reason, doesn't work
                                    //when setting it to null and checking against that.
                    else
                        _Name = value;

                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        private string _Description;

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

        private ICommand _CloseCommand;

        public ICommand CloseCommand
        {
            get
            {
                return _CloseCommand ?? (_CloseCommand = new RelayCommand<IDialogWindow>((window) => {

                    Item.Name = Name;
                    Item.Description = Description;

                    CloseDialogWithResult(window, Item);
                }));
            }
        }

        private ITwoField _Item;

        public ITwoField Item
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

                    Name = value.Name;
                    Description = value.Description;

                    RaisePropertyChanged(nameof(Item));
                }
            }
        }

    }
}