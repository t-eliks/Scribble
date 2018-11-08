namespace Scribble.Logic
{
    using System.Windows;
    using System.Windows.Media.Imaging;

    public static class IconHelper
    {
        public static BitmapSource FindIconInResources(string resourceKey)
        {
            return (BitmapSource)Application.Current.TryFindResource(resourceKey);
        }
    }
}
