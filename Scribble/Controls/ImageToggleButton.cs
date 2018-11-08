namespace Scribble.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    public class ImageToggleButton : ToggleButton
    {
        public ImageToggleButton()
        {
            this.Click += (s, e) =>
            {
                if ((bool)IsChecked)
                {
                    Image = OnImage;
                }
                else
                    Image = OffImage;
            };

            this.Initialized += (s, e) => { Image = (bool)IsChecked ? OnImage : OffImage; };

            this.Checked += (s, e) => { Image = OnImage; };

            this.Unchecked += (s, e) => { Image = OffImage; };
        }

        public static readonly DependencyProperty OffImageProperty = DependencyProperty.Register("OffImage",
            typeof(ImageSource), typeof(ImageToggleButton));

        public ImageSource OffImage
        {
            get { return (ImageSource)GetValue(OffImageProperty); }
            set { SetValue(OffImageProperty, value); }
        }

        public static readonly DependencyProperty OnImageProperty = DependencyProperty.Register("OnImage",
            typeof(ImageSource), typeof(ImageToggleButton));

        public ImageSource OnImage
        {
            get { return (ImageSource)GetValue(OnImageProperty); }
            set { SetValue(OnImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image",
            typeof(ImageSource), typeof(ImageToggleButton));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }
}
