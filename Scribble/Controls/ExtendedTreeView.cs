 namespace Scribble.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ExtendedTreeView : TreeView
    {
        public ExtendedTreeView()
            : base()
        {
            this.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(ChangeSelectedItem);
            this.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
        }

        public static readonly DependencyProperty _SelectedItemProperty = DependencyProperty.Register("_SelectedItem", 
            typeof(object), typeof(ExtendedTreeView), new UIPropertyMetadata(null));

        public object _SelectedItem
        {
            get { return (object)GetValue(_SelectedItemProperty); }
            set { SetValue(_SelectedItemProperty, value); }
        }

        void ChangeSelectedItem(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedItem != null)
            {
                SetValue(_SelectedItemProperty, SelectedItem);
            }
        }

        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
    }
}
