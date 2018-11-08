namespace Scribble.Controls
{
    using Scribble.Models;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class TimelineContent : ContentControl
    {
        public TimelineContent(Scene scene)
        {
            Scene = scene;
            this.Content = scene;
        }

        public TimelineContent() { }

        public static readonly DependencyProperty SceneProperty = DependencyProperty.Register("Scene",
            typeof(Scene), typeof(TimelineContent), new FrameworkPropertyMetadata(null)
            {
                PropertyChangedCallback = (s, e) =>
                {
                    if (e.NewValue != null)
                    {
                        var scene = e.NewValue as Scene;
                        var content = s as TimelineContent;

                        content.CanvasLeft = scene.CanvasLeft;
                        content.CanvasTop = scene.CanvasTop;
                    }
                }
            });

        public Scene Scene
        {
            get { return (Scene)GetValue(SceneProperty); }
            set { SetValue(SceneProperty, value); }
        }

        public static readonly DependencyProperty CanvasLeftProperty = DependencyProperty.Register("CanvasLeft",
            typeof(double), typeof(TimelineContent), new FrameworkPropertyMetadata(0.0) {
                PropertyChangedCallback = (s, e) => 
            {
                if ((double)e.NewValue >= 0)
                {
                    Canvas.SetLeft((TimelineContent)s, (double)e.NewValue);
                    ((TimelineContent)s).Scene.CanvasLeft = (double)e.NewValue;
                }

            }
            });

        public double CanvasLeft
        {
            get { return (double)GetValue(CanvasLeftProperty); }
            set { SetValue(CanvasLeftProperty, value); }
        }

        public static readonly DependencyProperty CanvasTopProperty = DependencyProperty.Register("CanvasTop",
            typeof(double), typeof(TimelineContent), new FrameworkPropertyMetadata(0.0)
            {
                PropertyChangedCallback = (s, e) =>
                {
                    if ((double)e.NewValue >= 0)
                    {
                        Canvas.SetTop((TimelineContent)s, (double)e.NewValue);
                        ((TimelineContent)s).Scene.CanvasTop = (double)e.NewValue;
                    }
                }
            });

        public double CanvasTop
        {
            get { return (double)GetValue(CanvasTopProperty); }
            set { SetValue(CanvasTopProperty, value); }
        }

        Point point;
        Point secondpoint;

        double newX = 0;
        double newY = 0;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            point = e.GetPosition(this);

            if (!this.IsMouseCaptured)
                this.CaptureMouse();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this.IsMouseCaptured)
            {
                secondpoint = e.GetPosition(this);
                TimelineCanvas c = this.Parent as TimelineCanvas;

                newX = CanvasLeft + (secondpoint.X - point.X);

                if (newX < 0)
                    newX = 0;
                else if (newX >= c.ActualWidth - this.ActualWidth)
                    c.Width += 50;

                this.CanvasLeft = newX;

                newY = CanvasTop + (secondpoint.Y - point.Y);

                if (newY < 0)
                    newY = 0;
                else if (newY >= c.ActualHeight - this.ActualHeight)
                    newY = c.ActualHeight - this.ActualHeight;

                this.CanvasTop = newY;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            var r = (int)(newY / this.ActualHeight) * this.ActualHeight;

            if (r < 0)
                r = 0;
            if (r >= ((TimelineCanvas)this.Parent).ActualHeight)
                r = ((TimelineCanvas)this.Parent).ActualHeight - this.ActualHeight;

            this.CanvasTop = r;

            ((TimelineCanvas)this.Parent).Resize();

            this.ReleaseMouseCapture();
        }
    }
}
