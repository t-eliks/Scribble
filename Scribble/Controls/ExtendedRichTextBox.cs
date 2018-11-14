namespace Scribble.Controls
{
    using Scribble.Models;
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
           typeof(TextFile), typeof(ExtendedRichTextBox), new FrameworkPropertyMetadata(null) { PropertyChangedCallback = (s, e) => 
           {
               RichTextBox rtb = s as RichTextBox;
               TextFile file = (TextFile)e.NewValue;

               if (file != null)
               {
                   var text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                   using (var stream = file.GetStream())
                   {
                       text.Load(stream, DataFormats.Rtf);
                   }
               }
           } });

        public TextFile TextFile
        {
            get { return (TextFile)GetValue(TextFileProperty); }
            set { SetValue(TextFileProperty, value); }
        }

        public static void Save(ExtendedRichTextBox rtb)
        {
            using (var stream = rtb.TextFile.GetStream())
            {
                var text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                text.Save(stream, DataFormats.Rtf);
                rtb.TextFile.SaveChangesToMainFile();
            }
        }
    }
}
