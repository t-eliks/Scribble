namespace Scribble
{
    using Scribble.Controls;
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
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

            MainViewModel.OnSceneViewChanged += (s, e) =>
            {
                foreach (var item in CollectDisposableChildren(new List<IDisposable>(), ((TabControlViewItem)cc.SelectedItem).Content))
                {
                    item.Dispose();
                }
            };

            cc.SelectionChanged += (s, e) => 
            {
                if (e.Source is TabControl && ((TabControl)s).SelectedItem is TabControlViewItem viewitem)
                {
                    ((IViewItemViewModel)(viewitem.Content.Content)).RefreshCommand.Execute(null);
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

        private ContentPresenter GetTargetTabItem(object originalSource)
        {
            var current = originalSource as DependencyObject;

            while (current != null)
            {
                var tabItem = current as ContentPresenter;
                if (tabItem != null)
                {
                    return tabItem;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }


        private void TabItem_Drag(object sender, MouseEventArgs e)
        {
            var tabItem = e.Source as ContentPresenter;

            if (tabItem == null)
                return;

            if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
                DragDrop.DoDragDrop(tabItem, tabItem, DragDropEffects.All);
            else
                e.Handled = true;
        }

        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            var tabItemTarget = GetTargetTabItem(e.OriginalSource);
            if (tabItemTarget != null)
            {
                var tabItemSource = (ContentPresenter)e.Data.GetData(typeof(ContentPresenter));
                if (tabItemTarget != tabItemSource)
                {
                    var viewitems = (ObservableCollection<TabControlViewItem>)cc.ItemsSource;

                    var source = (TabControlViewItem)tabItemSource.Content;
                    var target = (TabControlViewItem)tabItemTarget.Content;

                    int sourceIndex = viewitems.IndexOf(source);
                    int targetIndex = viewitems.IndexOf(target);

                    viewitems.Remove(source);
                    viewitems.Insert(targetIndex, source);
                    
                    viewitems.Remove(target);
                    viewitems.Insert(sourceIndex, target);

                    cc.SelectedIndex = targetIndex;

                    e.Handled = true;
                }
            }
        }
    }
}
