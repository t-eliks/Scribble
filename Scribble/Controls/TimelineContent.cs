namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class TimelineContent : ContentControl
    {
        public TimelineContent()
        {
            this.MouseLeave += delegate { Mouse.OverrideCursor = null; };
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register("Model",
            typeof(CanvasItemModel), typeof(TimelineContent), new FrameworkPropertyMetadata(null));

        public CanvasItemModel Model
        {
            get { return (CanvasItemModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        public static readonly DependencyProperty TimelineCanvasProperty = DependencyProperty.Register("TimelineCanvas",
            typeof(TimelineCanvas), typeof(TimelineContent), new FrameworkPropertyMetadata(null));

        public TimelineCanvas TimelineCanvas
        {
            get { return (TimelineCanvas)GetValue(TimelineCanvasProperty); }
            set { SetValue(TimelineCanvasProperty, value); }
        }

        Point point;
        Point secondpoint;

        double newX = 0;
        double newY = 0;

        bool WidthResizeMode = false;
        double previousWidth = 0;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            point = e.GetPosition(this);

            if (!this.IsMouseCaptured)
            {
                if (point.X >= (this.ActualWidth - 5))
                {
                    WidthResizeMode = true;
                    previousWidth = this.ActualWidth;
                }

                this.CaptureMouse();
            }
                
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this.IsMouseCaptured)
            {
                if (!WidthResizeMode)
                {
                    secondpoint = e.GetPosition(this);

                    newX = Model.CanvasLeft + (secondpoint.X - point.X);

                    if (newX < 0)
                        newX = 0;

                    Model.CanvasLeft = newX;

                    newY = Model.CanvasTop + (secondpoint.Y - point.Y);

                    if (newY < 0)
                        newY = 0;
                    else if (newY >= TimelineCanvas.ActualHeight - TimelineCanvas.RowHeight)
                        newY = TimelineCanvas.ActualHeight - TimelineCanvas.RowHeight;

                    Model.CanvasTop = newY;
                }
                else
                {
                    secondpoint = e.GetPosition(this);

                    if (previousWidth + secondpoint.X - point.X >= 10)
                        this.Model.Width = previousWidth + secondpoint.X - point.X;
                }
            }
            else
            {
                var position = e.GetPosition(this);

                if (position.X >= (this.ActualWidth - 5))
                    Mouse.OverrideCursor = Cursors.SizeWE;
                else
                    Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            var r = (int)((Model.CanvasTop+this.ActualHeight/2) / TimelineCanvas.RowHeight) * TimelineCanvas.RowHeight;

            Point pt = e.GetPosition(TimelineCanvas);

            Model.CanvasTop = r;

            if (WidthResizeMode)
                WidthResizeMode = false;

            this.ReleaseMouseCapture();
        }
    }
}