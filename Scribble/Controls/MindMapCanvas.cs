namespace Scribble.Controls
{
    using Scribble.Interfaces;
    using Scribble.Models;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Linq;
    using System.Windows.Shapes;
    using Scribble.ViewModels.DialogViewModels;
    using Scribble.ViewModels;

    public class MindMapCanvas : Canvas
    {
        public MindMapCanvas()
        {
            this.Width = 3072;
            this.Height = 2048;

            this.SnapsToDevicePixels = true;

            this.MouseWheel += (s, e) =>
            {
                e.Handled = true;

                if ((double)e.Delta < 0 && Scale - 0.2 >= 0.4)
                    Scale -= 0.2;
                if ((double)e.Delta > 0 && Scale + 0.2 <= 1.6)
                    Scale += 0.2;
            };
            this.Focus();

            this.Loaded += (s, e) => { LoadSerializedLines(); Status = DefaultStatus; };

        }

        private const string DefaultStatus = "To interact, right-click anywhere on the map. Zoom-in with mouse wheel.";

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale",
            typeof(double), typeof(MindMapCanvas), new FrameworkPropertyMetadata(1.0) { PropertyChangedCallback = (s, e) => 
            {
                var canvas = (MindMapCanvas)s;

                canvas.LayoutTransform = new ScaleTransform((double)e.NewValue, (double)e.NewValue);
            }});

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem",
            typeof(MindMapItemModel), typeof(MindMapCanvas), new FrameworkPropertyMetadata(null));

        public MindMapItemModel SelectedItem
        {
            get { return (MindMapItemModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty CustomizeModeProperty = DependencyProperty.Register("CustomizeMode",
            typeof(bool), typeof(MindMapCanvas), new FrameworkPropertyMetadata(false));

        public bool CustomizeMode
        {
            get { return (bool)GetValue(CustomizeModeProperty); }
            set { SetValue(CustomizeModeProperty, value); }
        }

        private bool ConnectMode { get; set; }

        private MindMapContent ConnectingItem { get; set; }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(ObservableCollection<MindMapItemModel>), typeof(MindMapCanvas), new FrameworkPropertyMetadata(null)
            {
                PropertyChangedCallback = (s, e) => {

                    MindMapCanvas canvas = (MindMapCanvas)s;

                    //canvas.Children.Clear();

                    var newContent = (ObservableCollection<MindMapItemModel>)e.NewValue;

                    var UpdateContent = new NotifyCollectionChangedEventHandler((sender, args) =>
                    {
                        if (canvas != null)
                        {
                            foreach (var item in newContent)
                            {
                                if (!canvas.Children.Contains(canvas.GetMindMapContent(item.MindMapItem)))
                                {
                                    MindMapContent content = new MindMapContent(item, canvas);

                                    content.Width = item.Width;
                                    content.Height = item.Height;

                                    if (content.CanvasLeft == 0 && content.CanvasTop == 0 && canvas.Parent is PanScrollViewer viewer)
                                    {
                                        content.CanvasLeft = viewer.HorizontalOffset;
                                        content.CanvasTop = viewer.VerticalOffset;
                                    }

                                    ContextMenu menu = new ContextMenu();

                                    MenuItem menuitem2 = new MenuItem() { Header = "Connect to", Template = App.Current.TryFindResource("SubmenuItem") as ControlTemplate };
                                    menuitem2.Click += (o, a) =>
                                    {
                                        canvas.ConnectMode = true;
                                        canvas.ConnectingItem = content;

                                        canvas.CustomizeMode = false;

                                        canvas.Status = "To connect, select another item. To cancel, right-click anywhere on the map.";
                                        
                                    };

                                    MenuItem menuitem3 = new MenuItem() { Header = "Remove", Template = App.Current.TryFindResource("SubmenuItem") as ControlTemplate };
                                    menuitem3.Click += (o, a) =>
                                    {
                                        if (MessageBox.Show("Are you sure? This cannot be undone. (This doesn't delete of the object itself, only the instance of it on this mind map.)", "Delete?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                                        {
                                            if (canvas.Content.Contains(item))
                                            {
                                                canvas.Content.Remove(item);
                                                canvas.Children.Remove(canvas.GetMindMapContent(item.MindMapItem));
                                                item.Remove();

                                                canvas.CustomizeMode = false;

                                                canvas.RefreshLines();
                                            }
                                        }
                                    };

                                    menu.Items.Add(menuitem2);
                                    menu.Items.Add(menuitem3);

                                    if (item.MindMapItem is MindMapString str)
                                    {
                                        MenuItem menuitem4 = new MenuItem() { Header = "Edit", Template = App.Current.TryFindResource("SubmenuItem") as ControlTemplate };
                                        menuitem4.Click += (o, a) => { var dialog = new TwoFieldInfoViewModel(true) { Item = str }; MainViewModel._DialogService.OpenDialog(dialog); };
                                        menu.Items.Add(menuitem4);
                                    }

                                    MenuItem menuitem5 = new MenuItem() { Header = "Customize", Template = App.Current.TryFindResource("SubmenuItem") as ControlTemplate };
                                    menuitem5.Click += (o, a) => 
                                    {
                                        canvas.SelectedItem = item;
                                        canvas.CustomizeMode = true;
                                    };
                                    menu.Items.Add(menuitem5);

                                    content.ContextMenu = menu;

                                    Panel.SetZIndex(content, 2);

                                    canvas.Children.Add(content);
                                }
                            }
                        }
                    });

                    if (e.OldValue != null)
                        ((ObservableCollection<MindMapItemModel>)e.OldValue).CollectionChanged -= UpdateContent;

                    if (newContent != null)
                        newContent.CollectionChanged += UpdateContent;

                    UpdateContent.Invoke(null, null);
                }
            });

        private void RefreshLines()
        {
            foreach (var item in Children.OfType<MindMapContent>().ToList())
            {
                item.RedrawLines();
            }
        }

        public ObservableCollection<MindMapItemModel> Content
        {
            get { return (ObservableCollection<MindMapItemModel>)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        private bool Contains(MindMapItemModel item)
        {
            foreach (var child in Children)
            {
                if (child is MindMapContent content && content.Content == item.MindMapItem)
                    return true;
            }

            return false;
        }

        public MindMapContent GetMindMapContent(IMindMapItem item)
        {
            foreach (var child in Children)
            {
                if (child is MindMapContent mc && mc.Item.MindMapItem == item)
                    return mc;
            }

            return null;
        }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status",
          typeof(string), typeof(MindMapCanvas), new FrameworkPropertyMetadata(null));

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public void LoadSerializedLines()
        {
            Collection<MindMapLineModel> linemodels = new Collection<MindMapLineModel>();

            foreach (var item in Children.OfType<MindMapContent>().ToList())
            {
                foreach (var linemodel in item.LineModels.ToList())
                {
                    if (!linemodels.Contains(linemodel))
                    {
                        item.AddLine(GetMindMapContent(linemodel.GetOpposite(item.Item).MindMapItem), linemodel);

                        linemodels.Add(linemodel);
                    }
                }
            }
        }

        private Line ConnectingLine;

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (ConnectMode && ConnectingItem != null)
            {
                if (this.Children.Contains(ConnectingLine))
                {
                    ConnectingLine.X2 = e.GetPosition(this).X;
                    ConnectingLine.Y2 = e.GetPosition(this).Y;
                }
                else
                {
                    Point relativePoint = ConnectingItem.TransformToAncestor(this).Transform(new Point(0, 0));
                    Point relativePoint2 = e.GetPosition(this);
                    Point pt1 = new Point(relativePoint.X + ConnectingItem.ActualWidth / 2, relativePoint.Y + ConnectingItem.ActualHeight / 2);

                    Line l = new Line();
                    l.Stroke = new SolidColorBrush(Color.FromRgb(71, 23, 246));
                    l.StrokeThickness = 5.0;
                    l.X1 = pt1.X;
                    l.X2 = relativePoint2.X;
                    l.Y1 = pt1.Y;
                    l.Y2 = relativePoint2.Y;

                    ConnectingLine = l;

                    this.Children.Add(ConnectingLine);
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (ConnectMode)
            {
                var outlinecontent = e.Source as MindMapContent;

                if (outlinecontent != null)
                {
                    if (ConnectingItem != null)
                    {
                        ConnectingItem.AddLine(outlinecontent);
                    }

                    ConnectMode = false;
                    ConnectingItem = null;

                    Status = DefaultStatus;

                    if (this.Children.Contains(ConnectingLine))
                        this.Children.Remove(ConnectingLine);
                    ConnectingLine = null;

                    e.Handled = true;
                }
            }

        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            if (ConnectMode)
            {
                ConnectMode = false;
                ConnectingItem = null;

                if (this.Children.Contains(ConnectingLine))
                    this.Children.Remove(ConnectingLine);

                ConnectingLine = null;

                Status = DefaultStatus;

                e.Handled = true;
            }

            base.OnPreviewMouseRightButtonDown(e);
        }
    }
}