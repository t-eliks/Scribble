namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    [Serializable]
    public class ProjectModel : BaseModel, ISerializable
    {
        public ProjectModel()
        {
            SymbioticLinks = new ObservableCollection<SymbioticLink>();

            CustomItemCount = 0;
        }

        public ProjectModel(string name, string author, Forms form, Genres genre, DateTime creationdate, string directory)
        {
            Name = name;
            Author = author;
            Form = form;
            Genre = genre;
            CreationDate = creationdate;
            CustomItemCount = 0;

            var sanitizeddirectory = Path.Combine(Path.GetDirectoryName(directory), Path.GetFileName(directory).Trim(Path.GetInvalidFileNameChars()));

            if (!Directory.Exists(sanitizeddirectory))
            {
                Directory.CreateDirectory(sanitizeddirectory);
                Directory.CreateDirectory(Path.Combine(sanitizeddirectory, "Files"));
                ProjectDirectory = sanitizeddirectory;
            }

            SymbioticLinks = new ObservableCollection<SymbioticLink>();

            Timelines = new ObservableCollection<TimelineModel>();
        }

        #region Properties and Fields

        private string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;

                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        private string _Author;

        public string Author
        {
            get
            {
                return _Author;
            }
            set
            {
                if (_Author != value)
                {
                    _Author = value;

                    RaisePropertyChanged(nameof(Author));
                }
            }
        }

        private string _Description;

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_Description != value)
                {
                    _Description = value;

                    RaisePropertyChanged(nameof(Description));
                }
            }
        }

        private int _CustomItemCount;

        public int CustomItemCount
        {
            get
            {
                return _CustomItemCount;
            }
            set
            {
                if (_CustomItemCount != value && value >= 0)
                {
                    _CustomItemCount = value;

                    RaisePropertyChanged(nameof(CustomItemCount));
                }
            }
        }

        public string CreationDateString { get { return CreationDate.ToString("yyyy/MM/dd"); } }

        private DateTime _CreationDate;

        public DateTime CreationDate
        {
            get
            {
                return _CreationDate;
            }
            set
            {
                if (_CreationDate != value)
                {
                    _CreationDate = value;

                    RaisePropertyChanged(nameof(CreationDate));
                }
            }
        }

        private Forms _Form;

        public Forms Form
        {
            get
            {
                return _Form;
            }
            set
            {
                if (_Form != value)
                {
                    _Form = value;

                    RaisePropertyChanged(nameof(Form));
                }
            }
        }

        private Genres _Genre;

        public Genres Genre
        {
            get
            {
                return _Genre;
            }
            set
            {
                if (_Genre != value)
                {
                    _Genre = value;

                    RaisePropertyChanged(nameof(Genre));
                }
            }
        }

        private string _ProjectDirectory;

        public string ProjectDirectory
        {
            get
            {
                return _ProjectDirectory;
            }
            set
            {
                if (_ProjectDirectory != value)
                {
                    _ProjectDirectory = value;

                    RaisePropertyChanged(nameof(ProjectDirectory));
                }
            }
        }

        public string FileDirectory { get { return Path.Combine(ProjectDirectory, "Files"); } }

        public ObservableCollection<BaseItem> ProjectFiles
        {
            get
            {
                return FindLinks<BaseItem>(this);
            }
        }

        private ObservableCollection<SymbioticLink> _SymbioticLinks;

        public ObservableCollection<SymbioticLink> SymbioticLinks
        {
            get
            {
                return _SymbioticLinks;
            }
            set
            {
                if (_SymbioticLinks != value)
                {
                    _SymbioticLinks = value;

                    RaisePropertyChanged(nameof(SymbioticLinks));
                }
            }
        }

        public string GenreString { get { return Enum.GetName(typeof(Genres), Genre); } }

        public string FormString { get { return Enum.GetName(typeof(Forms), Form); } }

        private ObservableCollection<TimelineModel> _Timelines;

        public ObservableCollection<TimelineModel> Timelines
        {
            get
            {
                return _Timelines;
            }
            set
            {
                if (_Timelines != value)
                {
                    _Timelines = value;

                    RaisePropertyChanged(nameof(Timelines));
                }
            }
        }

        private ObservableCollection<MindMapModel> _MindMaps;

        public ObservableCollection<MindMapModel> MindMaps
        {
            get
            {
                return _MindMaps ?? (_MindMaps = new ObservableCollection<MindMapModel>());
            }
            set
            {
                if (_MindMaps != value)
                {
                    _MindMaps = value;

                    RaisePropertyChanged(nameof(MindMaps));
                }
            }
        }

        #endregion

        #region Methods

        public void AddItem(Item item)
        {
            AddSymbioticLink(new SymbioticLink<ProjectModel, Item>(this, item));
        }

        public void AddSymbioticLink<A, B>(SymbioticLink<A, B> link) where A : BaseModel where B: BaseModel
        {
            SymbioticLinks.Add(link);
        }

        public ObservableCollection<X> FindLinks<X>(object item)
        {
            var results = new ObservableCollection<X>();

            foreach (var link in SymbioticLinks)
            {
                var result = link.CheckLink<X>(item);

                if (result != null && !result.Equals(default(X)))
                    results.Add(result);
            }

            return results;
        }

        public ObservableCollection<X> FindBiLinks<X>(object item)
        {
            var results = new ObservableCollection<X>();

            foreach (var link in SymbioticLinks)
            {
                var result = link.CheckBiLink<X>(item);

                if (result != null && !result.Equals(default(X)) && !results.Contains(result))
                    results.Add(result);
            }

            return results;
        }

        public ObservableCollection<Item> ProjectItems()
        {
            var files = new ObservableCollection<Item>();

            foreach (var link in SymbioticLinks)
            {
                object child;

                object parent = link.GetObjects(out child);

                if (!files.Contains(parent) && !(parent is ProjectFolder) && parent is Item)
                    files.Add((Item)parent);

                if (!files.Contains(child) && !(child is ProjectFolder) && child is Item)
                    files.Add((Item)child);
            }

            return files;
        }

        public ObservableCollection<Item> Search(string query, int itemlimit)
        {
            var results = new ObservableCollection<Item>();

            foreach (ISearchable item in ProjectItems())
            {
                if (item.CheckMatch(query))
                    results.Add((Item)item);

                if (itemlimit > 0 && results.Count >= itemlimit)
                    break;
            }

            return results;
        }

        public void DeleteItemBiLinks(object item)
        {
            foreach (var link in SymbioticLinks.ToList())
            {
                if (link.IsBiLink(item))
                    SymbioticLinks.Remove(link);
            }
        }

        public void DeleteItemLinks(object parent, object child)
        {
            foreach (var link in SymbioticLinks.ToList())
            {
                if (link.IsLink(parent, child))
                    SymbioticLinks.Remove(link);
            }
        }

        public ObservableCollection<X> GetItemsOfType<X>()
        {
            var results = new ObservableCollection<X>();

            foreach (var link in SymbioticLinks)
            {
                var result = link.GetItemOfType<X>();

                if (result != null && !result.Equals(default(X)) && !results.Contains(result))
                    results.Add(result);
            }

            return results;
        }

        public void DeleteItem(Item item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Serialization

        protected ProjectModel(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
            Author = info.GetString("author");
            Description = info.GetString("description");
            Genre = (Genres)info.GetValue("genre", typeof(Genres));
            Form = (Forms)info.GetValue("form", typeof(Forms));
            CreationDate = info.GetDateTime("creationdate");
            CustomItemCount = info.GetInt32("customitemcount");
            ProjectDirectory = info.GetString("projectdirectory");
            SymbioticLinks = (ObservableCollection<SymbioticLink>)info.GetValue("symbioticlinks", typeof(ObservableCollection<SymbioticLink>));
            Timelines = (ObservableCollection<TimelineModel>)info.GetValue("timelines", typeof(ObservableCollection<TimelineModel>));
            MindMaps = (ObservableCollection<MindMapModel>)info.GetValue("outlines", typeof(ObservableCollection<MindMapModel>));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("author", Author);
            info.AddValue("description", Description);
            info.AddValue("genre", Genre);
            info.AddValue("form", Form);
            info.AddValue("creationdate", CreationDate);
            info.AddValue("customitemcount", CustomItemCount);
            info.AddValue("projectdirectory", ProjectDirectory);
            info.AddValue("symbioticlinks", SymbioticLinks);
            info.AddValue("timelines", Timelines);
            info.AddValue("outlines", MindMaps);
        }

        #endregion
    }

    public enum Forms
    {
        Novel,
        Poem,
        Drama,
        Novella
    }

    public enum Genres
    {
        Comedy,
        Drama,
        Epic,
        Erotic,
        Nonsense,
        Lyric,
        Mythopoeia,
        Romance,
        Satire,
        Tragedy,
        Tragicomedy
    }
}