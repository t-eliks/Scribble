namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class MindMapContent : ContentControl
    {
        public MindMapContent(MindMapItemModel item, MindMapCanvas parentCanvas)
        {
            Item = item;
            this.Content = item.MindMapItem;

            ParentCanvas = parentCanvas;
            CanvasLeft = item.CanvasLeft;
            CanvasTop = item.CanvasTop;
            
        }

        public MindMapCanvas ParentCanvas { get; private set; }

        public Collection<MindMapLine> Lines { get; private set; } = new ObservableCollection<MindMapLine>();

        public MindMapItemModel Item { get; private set; }

        public Collection<MindMapLineModel> LineModels
        {
            get
            {
                return Item.Lines;
            }
        }

        public static readonly DependencyProperty CanvasLeftProperty = DependencyProperty.Register("CanvasLeft",
            typeof(double), typeof(MindMapContent), new FrameworkPropertyMetadata(0.0)
            {
                PropertyChangedCallback = (s, e) =>
                {
                    if ((double)e.NewValue >= 0)
                    {
                        Canvas.SetLeft((MindMapContent)s, (double)e.NewValue);
                        ((MindMapContent)s).Item.CanvasLeft = (double)e.NewValue;

                        ((MindMapContent)s).RedrawLines();
                    }

                }
            });

        public double CanvasLeft
        {
            get { return (double)GetValue(CanvasLeftProperty); }
            set { SetValue(CanvasLeftProperty, value); }
        }

        public static readonly DependencyProperty CanvasTopProperty = DependencyProperty.Register("CanvasTop",
            typeof(double), typeof(MindMapContent), new FrameworkPropertyMetadata(0.0)
            {
                PropertyChangedCallback = (s, e) =>
                {
                    if ((double)e.NewValue >= 0)
                    {
                        Canvas.SetTop((MindMapContent)s, (double)e.NewValue);
                        ((MindMapContent)s).Item.CanvasTop = (double)e.NewValue;

                        ((MindMapContent)s).RedrawLines();
                    }
                }
            });

        public void AddLine(MindMapContent content2)
        {
            AddLine(content2, "New line", "No description.");
        }

        public void AddLine(MindMapContent content2, string header, string description)
        {
            bool linkexists = false;

            foreach (var line in Lines)
            {
                if (line.IsLink(this, content2))
                {
                    linkexists = true;
                    break;
                }
            }

            if (!linkexists)
            {
                var l = new MindMapLine(this, content2) { ParentCanvas = ParentCanvas, Header = header, Description = description };

                this.Lines.Add(l);
                content2.Lines.Add(l);

                l.RedrawLine();
            }
        }

        public void RedrawLines()
        {
            foreach (var line in Lines)
            {
                line.RedrawLine();
            }
        }

        public double CanvasTop
        {
            get { return (double)GetValue(CanvasTopProperty); }
            set { SetValue(CanvasTopProperty, value); }
        }

        Point point;
        Point secondpoint;

        double newX = 0;
        double newY = 0;

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            point = e.GetPosition(this);

            if (!this.IsMouseCaptured)
                this.CaptureMouse();
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (this.IsMouseCaptured)
            {
                secondpoint = e.GetPosition(this);
                MindMapCanvas c = this.Parent as MindMapCanvas;

                newX = CanvasLeft + (secondpoint.X - point.X);

                if (newX < 0)
                    newX = 0;
                else if (newX >= c.ActualWidth - this.ActualWidth)
                    newX = c.ActualWidth - this.ActualWidth;

                this.CanvasLeft = newX;

                newY = CanvasTop + (secondpoint.Y - point.Y);

                if (newY < 0)
                    newY = 0;
                else if (newY >= c.ActualHeight - this.ActualHeight)
                    newY = c.ActualHeight - this.ActualHeight;

                this.CanvasTop = newY;
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            this.ReleaseMouseCapture();
        }
    }
}
