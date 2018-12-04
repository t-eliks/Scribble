namespace Scribble
{
    using Scribble.Controls;
    using Scribble.Logic;
    using Scribble.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;

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

            ((MainViewModel)this.DataContext).OnSceneViewChanged += (s, e) =>
            {
                foreach (var item in CollectDisposableChildren(new List<IDisposable>(), ((TabControlViewItem)cc.SelectedItem).Content))
                {
                    item.Dispose();
                }
            };
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

        private List<IDisposable> CollectDisposableChildren(List<IDisposable> disposable, DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child is IDisposable)
                    disposable?.Add((IDisposable)child);

                if (VisualTreeHelper.GetChildrenCount(child) > 0)
                    CollectDisposableChildren(disposable, child);
            }

            return disposable;
        }
    }
}
