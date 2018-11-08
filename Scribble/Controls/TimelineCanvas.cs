namespace Scribble.Controls
{
    using Scribble.Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    public class TimelineCanvas : Canvas
    {
        public TimelineCanvas()
        {
            this.Height = ItemHeight * Rows;
        }

        public void SubtractRow()
        {
            if (this.Height > ItemHeight)
                foreach (TimelineContent item in Children)
                {
                    if (item.CanvasTop >= Height - ItemHeight)
                    {
                        Storyboard sb = new Storyboard();

                        sb.Completed += (s, e) => { sb.Remove(); };

                        var anim = new DoubleAnimation()
                        {
                            From = item.CanvasTop,
                            To = item.CanvasTop - ItemHeight,
                            Duration = TimeSpan.FromMilliseconds(150),
                            AccelerationRatio = 0.4,
                            EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseIn }
                        };

                        Storyboard.SetTarget(anim, item);
                        Storyboard.SetTargetProperty(anim, new PropertyPath(TimelineContent.CanvasTopProperty));

                        sb.Children.Add(anim);

                        sb.Begin();

                        item.CanvasTop -= ItemHeight;
                    }
                }
        }

        public void Resize()
        {
            var resize = true;

            var amount = int.MaxValue;

            foreach (TimelineContent item in Children)
            {
                if (item.CanvasLeft + item.ActualWidth >= Width)
                    resize = false;
                else if (item.CanvasLeft + item.ActualWidth - this.Width < amount)
                    amount = (int)Math.Abs((item.CanvasLeft + item.ActualWidth - this.Width));
            }

            if (resize)
            {
                Storyboard sb = new Storyboard();

                sb.Completed += (sender, args) => { sb.Remove(); };

                var border = VisualTreeHelper.GetParent(this.Parent) as Border;

                var animborder = new DoubleAnimation()
                {
                    From = border.ActualWidth,
                    To = border.ActualWidth - amount,
                    Duration = TimeSpan.FromMilliseconds(150),
                    AccelerationRatio = 0.4,
                    EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseIn }
                };

                var animcanvas = new DoubleAnimation()
                {
                    From = ActualWidth,
                    To = ActualWidth - amount,
                    Duration = TimeSpan.FromMilliseconds(150),
                    AccelerationRatio = 0.4,
                    EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseIn }
                };

                Storyboard.SetTarget(animborder, border);
                Storyboard.SetTargetProperty(animborder, new PropertyPath(WidthProperty));

                Storyboard.SetTarget(animcanvas, this);
                Storyboard.SetTargetProperty(animcanvas, new PropertyPath(WidthProperty));

                sb.Children.Add(animborder);
                sb.Children.Add(animcanvas);

                sb.Begin();

                this.Width -= amount;
            }
        }

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight",
            typeof(int), typeof(TimelineCanvas), new FrameworkPropertyMetadata(0));

        public int ItemHeight
        {
            get { return (int)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows",
            typeof(int), typeof(TimelineCanvas), new FrameworkPropertyMetadata(0) { PropertyChangedCallback = (s, e) => 
            {
                if ((int)e.OldValue != 0 && (int)e.NewValue < (int)e.OldValue)
                    ((TimelineCanvas)s).SubtractRow();

                    var canvas = (TimelineCanvas)s;

                    Storyboard sb = new Storyboard();

                    var anim = new DoubleAnimation()
                    {
                        From = canvas.ItemHeight * (int)e.OldValue,
                        To = canvas.ItemHeight * (int)e.NewValue,
                        Duration = TimeSpan.FromMilliseconds(150),
                        AccelerationRatio = 0.4,
                        EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseIn }
                    };

                    Storyboard.SetTarget(anim, canvas);
                    Storyboard.SetTargetProperty(anim, new PropertyPath(HeightProperty));

                    sb.Children.Add(anim);

                    sb.Begin();   
            }});

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(ObservableCollection<Scene>), typeof(TimelineCanvas), new FrameworkPropertyMetadata(null) { PropertyChangedCallback = (s, e) => {

                TimelineCanvas canvas = (TimelineCanvas)s;

                var newContent = (ObservableCollection<Scene>)e.NewValue;

                var UpdateContent = new NotifyCollectionChangedEventHandler((sender, args) => 
                {
                    if (canvas != null)
                    {
                        canvas.Children.Clear();

                        foreach (var item in newContent)
                        {
                            ContextMenu menu = new ContextMenu();
                            MenuItem menuitem = new MenuItem() { Header = "Remove" };
                            menuitem.Click += (o, a) => 
                            {
                                if (canvas.Content.Contains(item))
                                    canvas.Content.Remove(item);
                            };

                            menu.Items.Add(menuitem);

                            canvas.Children.Add(new TimelineContent(item) { ContextMenu = menu });
                        }

                        
                    }
                });

                if (e.OldValue != null)
                    ((ObservableCollection<Scene>)e.OldValue).CollectionChanged -= UpdateContent;

                if (newContent != null)
                    newContent.CollectionChanged += UpdateContent;

                UpdateContent.Invoke(null, null);
            } });

        public ObservableCollection<Scene> Content
        {
            get { return (ObservableCollection<Scene>)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
    }
}
