namespace Scribble.ViewModels.DialogViewModels
{
    using Scribble.Logic;
    using Scribble.Models;
    using Scribble.Views.DialogViewModels;
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class PreferencesViewModel : DialogViewModelBase<object>
    {
        public PreferencesViewModel()
        {

        }

        private ICommand _ChangeThemeCommand;

        public ICommand ChangeThemeCommand
        {
            get
            {
                return _ChangeThemeCommand ?? (_ChangeThemeCommand = new RelayCommand(() => {
                    if (Theme != null)
                        Application.Current.Resources.MergedDictionaries[0].Source = new Uri(Theme.File);
                }));
            }
        }

        private Theme _Theme;

        public Theme Theme
        {
            get
            {
                return _Theme;
            }
            set
            {
                if (_Theme != value)
                {
                    _Theme = value;

                    ChangeThemeCommand.Execute(null);

                    RaisePropertyChanged(nameof(Theme));
                }
            }
        }
    }
}
