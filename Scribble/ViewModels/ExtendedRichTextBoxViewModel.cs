using System.Windows;

namespace Scribble.ViewModels
{
    public class ExtendedRichTextBoxViewModel : FrameworkElement
    {
        public static readonly DependencyProperty TextFilePathProperty = DependencyProperty.Register("TextFilePath",
            typeof(string), typeof(ExtendedRichTextBoxViewModel), new FrameworkPropertyMetadata(null));

        public string TextFilePath
        {
            get { return (string)GetValue(TextFilePathProperty); }
            set { SetValue(TextFilePathProperty, value); }
        }
    }
}
