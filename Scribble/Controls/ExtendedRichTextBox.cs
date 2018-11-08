namespace Scribble.Controls
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    public class ExtendedRichTextBox : RichTextBox
    {
        public ExtendedRichTextBox()
        {

        }

        public static readonly DependencyProperty TextFileProperty = DependencyProperty.Register("TextFile",
           typeof(string), typeof(ExtendedRichTextBox), new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true, PropertyChangedCallback = (s, e) => 
           {
               RichTextBox rtb = s as RichTextBox;
               string file = (string)e.NewValue;

               if (!String.IsNullOrEmpty(file) && File.Exists(file))
               {
                   var text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                   using (var stream = new FileStream(file, FileMode.Open))
                   {
                       text.Load(stream, DataFormats.Rtf);
                   }
               }
           } });

        public string TextFile
        {
            get { return (string)GetValue(TextFileProperty); }
            set { SetValue(TextFileProperty, value); }
        }

        public static void Save(ExtendedRichTextBox rtb)
        {
            using (var stream = new FileStream(rtb.TextFile, FileMode.Open))
            {
                var text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                text.Save(stream, DataFormats.Rtf);
            }
        }
    }
}
