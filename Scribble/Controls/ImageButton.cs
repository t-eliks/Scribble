namespace Scribble.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ImageButton : Button
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",
            typeof(ImageSource), typeof(ImageButton));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight",
            typeof(int), typeof(ImageButton));

        public int ImageHeight
        {
            get { return (int)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth",
            typeof(int), typeof(ImageButton));

        public int ImageWidth
        {
            get { return (int)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }
    }
}
