namespace Scribble.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class PanScrollViewer : ScrollViewer
    {
        public PanScrollViewer()
        {

        }

        Point point;
        Point offset;

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            point = e.GetPosition(this);
            offset.X = HorizontalOffset;
            offset.Y = VerticalOffset;
            

            if (!this.IsMouseCaptured)
                this.CaptureMouse();

            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                Point secondpoint = e.GetPosition(this);

                this.ScrollToHorizontalOffset(offset.X + (point.X - secondpoint.X));
                this.ScrollToVerticalOffset(offset.Y + (point.Y - secondpoint.Y));
            }

            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();

            base.OnMouseLeftButtonUp(e);
        }
    }

}
