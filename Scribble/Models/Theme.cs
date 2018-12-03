namespace Scribble.Models
{
    public class Theme
    {
        public Theme(string name, string file)
        {
            Name = name;
            File = file;
        }

        public Theme()
        {

        }

        public string Name { get; set; }

        public string File { get; set; }
    }
}