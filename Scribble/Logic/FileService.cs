using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Scribble.Logic
{
    public static class FileService
    {
        public static string GenerateEmptyRtf(string directory)
        {
            var filename = Path.GetRandomFileName() + ".rtf";

            while (File.Exists(filename))
            {
                filename = Path.GetRandomFileName() + ".rtf";
            }

            var rtf = new RichTextBox();

            rtf.FontFamily = new FontFamily("Times New Roman");
            rtf.FontSize = 16;

            var r = rtf.Selection;

            using (FileStream stream = new FileStream(Path.Combine(directory, filename), FileMode.Create))
            {
                r.Save(stream, DataFormats.Rtf);
            }

            return filename;
        }
    }
}
