namespace Scribble.Views.DialogViews
{
    using Scribble.Interfaces;
    using System.Windows;

    public partial class BaseDialogWindow : Window, IDialogWindow
    {
        public BaseDialogWindow()
        {
            InitializeComponent();

            exitBtn.Click += (s, e) => { this.Close(); };
        }
    }
}
