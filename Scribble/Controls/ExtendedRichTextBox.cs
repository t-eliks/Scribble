namespace Scribble.Controls
{
    using Scribble.Models;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Threading;

    public class ExtendedRichTextBox : RichTextBox, IDisposable
    {
        public ExtendedRichTextBox()
        {
            dt = new DispatcherTimer();
            dt.Tick += (s,e ) => 
            {
                if (_ChangedSinceSave)
                {
                    Save(this);
                    Status = "Last autosaved at " + DateTime.Now.ToString("HH:mm:ss");
                    _ChangedSinceSave = false;
                }
            };
            dt.Interval = new TimeSpan(0, 0, 5);
            dt.Start();

            this.TextChanged += (s, e) =>
            {
                var range = new TextRange(Document.ContentStart, Document.ContentEnd);
                char[] delimiters = new char[] { ' ', '\r', '\n' };
                WordCount = range.Text.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries).Length;
                if (!_ChangedSinceSave)
                {
                    _ChangedSinceSave = true;
                    Status = "Not saved";
                }
            };
        }

        DispatcherTimer dt;

        public static readonly DependencyProperty TextFileProperty = DependencyProperty.Register("TextFile",
           typeof(TextFile), typeof(ExtendedRichTextBox), new FrameworkPropertyMetadata(null) { PropertyChangedCallback = (s, e) => 
           {
               ExtendedRichTextBox rtb = s as ExtendedRichTextBox;
               TextFile file = (TextFile)e.NewValue;

               if (file != null)
               {
                   var text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                   using (var stream = file.GetStream())
                   {
                       text.Load(stream, DataFormats.Rtf);
                   }
               }

               rtb._ChangedSinceSave = false;
               rtb.Status = "Idle";
           } });

        public TextFile TextFile
        {
            get { return (TextFile)GetValue(TextFileProperty); }
            set { SetValue(TextFileProperty, value); }
        }

        public static readonly DependencyProperty WordCountProperty = DependencyProperty.Register("WordCount",
           typeof(int), typeof(ExtendedRichTextBox), new FrameworkPropertyMetadata(null));

        public int WordCount
        {
            get { return (int)GetValue(WordCountProperty); }
            set { SetValue(WordCountProperty, value); }
        }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status",
           typeof(string), typeof(ExtendedRichTextBox), new FrameworkPropertyMetadata(null));

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        private bool _ChangedSinceSave { get; set; }

        public static void Save(ExtendedRichTextBox rtb)
        {
           if (rtb.TextFile != null)
                using (var stream = rtb.TextFile.GetStream())
                {
                    var text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                    text.Save(stream, DataFormats.Rtf);
                    rtb.TextFile.SaveChangesToMainFile();
                    rtb.Status = "Saved at " + DateTime.Now.ToString("HH:mm:ss");
                }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                dt.Stop();
                dt = null;

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
