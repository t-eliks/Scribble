namespace Scribble.Controls
{
    using Scribble.Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    public class TimelineCanvas : Canvas
    {
        public TimelineCanvas()
        {
            this.Width = 3096;
            this.Height = 160;
        }

        public const int RowHeight = 80;

        public void SubtractRow()
        {
            if (this.Height > RowHeight)
                foreach (ContentPresenter cp in Children)
                {
                    var item = (CanvasItemModel)cp.Content;

                    if (Canvas.GetTop(cp) >= Height - RowHeight)
                    {
                        Storyboard sb = new Storyboard();

                        sb.Completed += (s, e) => { sb.Remove(); };

                        var anim = new DoubleAnimation()
                        {
                            From = GetTop(cp),
                            To = GetTop(cp) - RowHeight,
                            Duration = TimeSpan.FromMilliseconds(150),
                            AccelerationRatio = 0.4,
                            EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseIn }
                        };

                        Storyboard.SetTarget(anim, cp);
                        Storyboard.SetTargetProperty(anim, new PropertyPath(TopProperty));

                        sb.Children.Add(anim);

                        sb.Begin();

                        item.CanvasTop -= RowHeight;
                    }
                }
        }

        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows",
        typeof(int), typeof(TimelineCanvas), new FrameworkPropertyMetadata(0)
        {
            PropertyChangedCallback = (s, e) =>
            {
                var canvas = (TimelineCanvas)s;

                var newvalue = (int)e.NewValue;

                if ((int)e.OldValue != 0 && newvalue < (int)e.OldValue)
                    canvas.SubtractRow();

                Storyboard sb = new Storyboard();

                var anim = new DoubleAnimation()
                {
                    From = TimelineCanvas.RowHeight * (int)e.OldValue,
                    To = TimelineCanvas.RowHeight * (int)e.NewValue,
                    Duration = TimeSpan.FromMilliseconds(150),
                    AccelerationRatio = 0.4,
                    EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseIn }
                };

                Storyboard.SetTarget(anim, canvas);
                Storyboard.SetTargetProperty(anim, new PropertyPath(HeightProperty));

                sb.Children.Add(anim);

                sb.Begin();
            }
        });

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }
    }
}
