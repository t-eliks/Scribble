namespace Scribble
{
    using Scribble.ViewModels;
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

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
            Application.Current.Resources.MergedDictionaries[0].Source = new System.Uri("Styles/NewColors.xaml", UriKind.RelativeOrAbsolute);
        }
    }
}
