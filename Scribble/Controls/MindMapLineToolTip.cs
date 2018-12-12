namespace Scribble.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class MindMapLineToolTip : ToolTip
    {
        public MindMapLineToolTip()
        {

        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
          typeof(string), typeof(MindMapLineToolTip), new FrameworkPropertyMetadata(null));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
          typeof(string), typeof(MindMapLineToolTip), new FrameworkPropertyMetadata(null));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
    }
}
