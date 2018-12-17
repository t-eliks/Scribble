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

            this.MouseLeave += (s, e) => { Mouse.OverrideCursor = null; };

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

        public void ChangeBackground(MindMapItemColors color)
        {
            Item.BackgroundColor = color;
            this.Background = MindMapItemModel.GetBrush(color);
        }

        public void AddLine(MindMapContent content2)
        {
            AddLine(content2, "New line", "No description.", MindMapItemColors.Charcoal);
        }

        public void AddLine(MindMapContent content2, string header, string description, MindMapItemColors color)
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
                var l = new MindMapLine(this, content2) { ParentCanvas = ParentCanvas, Name = header, Description = description, Color = color};

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

        private bool WidthResizeMode = false;
        private bool HeightResizeMode = false;
        private double previousWidth = 0.0;
        private double previousHeight = 0.0;

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            point = e.GetPosition(this);

            if (!this.IsMouseCaptured)
            {
                if (point.X >= (this.ActualWidth - 5))
                {
                    WidthResizeMode = true;
                    previousWidth = this.Width;
                }
                else if (point.Y >= (this.ActualHeight - 5))
                {
                    HeightResizeMode = true;
                    previousHeight = this.Height;
                }
                this.CaptureMouse();
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (this.IsMouseCaptured)
            {
                if (!WidthResizeMode && !HeightResizeMode)
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
                else if (WidthResizeMode)
                {
                    secondpoint = e.GetPosition(this);

                    if (previousWidth + secondpoint.X - point.X >= 70)
                    {
                        this.Width = previousWidth + secondpoint.X - point.X;
                        this.Item.Width = previousWidth + secondpoint.X - point.X;
                    }
                }
                else if (HeightResizeMode)
                {
                    secondpoint = e.GetPosition(this);

                    if (previousHeight + secondpoint.Y - point.Y >= 50)
                    {
                        this.Height = previousHeight + secondpoint.Y - point.Y;
                        this.Item.Height = previousHeight + secondpoint.Y - point.Y;
                    }
                }
            }
            else
            {
                var position = e.GetPosition(this);

                if (position.X >= (this.ActualWidth - 5))
                {
                    Mouse.OverrideCursor = Cursors.SizeWE;
                }
                else if (position.Y >= (this.ActualHeight - 5))
                {
                    Mouse.OverrideCursor = Cursors.SizeNS;
                }
                else
                    Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            if (WidthResizeMode)
                WidthResizeMode = false;
            if (HeightResizeMode)
                HeightResizeMode = false;

            this.ReleaseMouseCapture();
        }
    }
}
