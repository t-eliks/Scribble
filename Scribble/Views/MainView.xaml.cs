namespace Scribble
{
    using Scribble.Logic;
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            minimiseBtn.Click += (s, e) => { this.WindowState = WindowState.Minimized; };

            maximiseBtn.Click += (s, e) => { this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal; };

            exitBtn.Click += (s, e) => { this.Close(); };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries[0].Source = new Uri("Styles/NewColors.xaml", UriKind.RelativeOrAbsolute);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (ProjectService.Instance.UnsavedChanges)
            {
                var result = MessageBox.Show("Save before exiting?", "Save", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                    ProjectService.Instance.SaveActiveProject();

                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }

                Environment.Exit(0);
            }
        }
    }
}
