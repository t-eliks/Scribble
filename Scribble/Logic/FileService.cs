using System.IO;

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

            using (StreamWriter writer = new StreamWriter(Path.Combine(directory, filename)))
            {
                writer.Write(" ");
            }

            return filename;
        }
    }
}
