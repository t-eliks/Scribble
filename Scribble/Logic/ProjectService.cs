namespace Scribble.Logic
{
    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    using Scribble.Models;
    using Scribble.ViewModels.DialogViewModels;
    using Scribble.ViewModels;

    public sealed class ProjectService
    {
        private ProjectService()
        {
            Projects = new ObservableCollection<ProjectModel>();

            LoadProjects();
        }

        public static ProjectService Instance { get; } = new ProjectService();

        #region Properties and Fields

        public string RootProjectsDirectory
        {
            get
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                return String.Concat(Directory.GetCurrentDirectory(), "\\", config.AppSettings.Settings["projectdirectory"].Value);
            }
        }

        public ProjectModel ActiveProject { get; private set; }

        public string ActiveProjectDirectory
        {
            get
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                return String.Concat(RootProjectsDirectory, "\\", ActiveProject.Name);
            }
        }

        public ObservableCollection<ProjectModel> Projects { get; set; }

        public bool UnsavedChanges { get; set; }

        #endregion

        #region Methods

        public void AddProject(ProjectModel project)
        {
            if (project != null)
            {
                Projects.Add(project);
                SaveProject(project);
            }
        }

        private void LoadProjects()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ObservableCollection<ProjectModel> projects = new ObservableCollection<ProjectModel>();

            if (!Directory.Exists(RootProjectsDirectory))
            {
                Directory.CreateDirectory(RootProjectsDirectory);
            }
            DirectoryInfo directory = new DirectoryInfo(RootProjectsDirectory);

            foreach (var folder in directory.GetDirectories())
            {
                projects.Add(SerializationService<ProjectModel>.DeserializeObject((Directory.GetFiles(folder.FullName).Where(x => Path.GetFileName(x).Contains("data.scribble")).First())));
            }

            Projects = projects != null ? projects : Projects;
        }

        public void SaveActiveProject()
        {
            SaveProject(ActiveProject);
        }

        public void SaveProject(ProjectModel project)
        {
            if (project != null)
            {
                SerializationService<ProjectModel>.SerializeObject(project, project.ProjectDirectory, "data.scribble");
                UnsavedChanges = false;
            }
        }

        public ProjectModel ShowSelectProjectDialog()
        {
            var dialog = new SelectProjectViewModel();
            
            var result = MainViewModel._DialogService.OpenDialog(dialog);

            if (result != null)
                ActiveProject = result;

            return result;
        }

        public void AddCharacter(ProjectFolder parentFolder)
        {
            parentFolder.AddItem(new Character("New Character", IconHelper.FindIconInResources("Character")));

            SaveActiveProject();
        }

        public void AddLocation(ProjectFolder parentFolder)
        {
            parentFolder.AddItem(new Location("New Location", IconHelper.FindIconInResources("Map")));

            SaveActiveProject();
        }

        public void AddFolder(ProjectFolder parentFolder)
        {
            parentFolder.AddItem(new ProjectFolder("New Folder", IconHelper.FindIconInResources("Folder")));

            SaveActiveProject();
        }

        public void AddScene(ProjectFolder parentFolder)
        {
            var scene = new Scene("New Scene", IconHelper.FindIconInResources("Pen"));

            parentFolder.AddItem(scene);

            SaveActiveProject();
        }

        public void RemoveItem(BaseItem projectItem)
        {
            projectItem.Delete();

            SaveActiveProject();
        }

        public void AddSymbioticLink<A, B>(SymbioticLink<A, B> link) where A : BaseModel where B : BaseModel
        {
            ActiveProject?.AddSymbioticLink(link);

            SaveActiveProject();
        }

        public ObservableCollection<X> FindLinks<X>(object item)
        {
            return ActiveProject?.FindLinks<X>(item);
        }

        public ObservableCollection<X> GetItemsOfType<X>()
        {
            return ActiveProject?.GetItemsOfType<X>();
        }

        public ObservableCollection<X> FindBiLinks<X>(object item)
        {
            return ActiveProject?.FindBiLinks<X>(item);
        }

        public ProjectModel CreateNewProject(string header, string author, string type)
        {
            return null;

            //var project = new ProjectModel(header, author, type, DateTime.Now, RootProjectsDirectory + $"\\{header}");

            //var root = new ProjectFolder(project, project.Name, IconHelper.FindIconInResources("Book"));
            //root.AddItem(new ProjectFolder(project, "Locations", IconHelper.FindIconInResources("Map")));
            //root.AddItem(new ProjectFolder(project, "Characters", IconHelper.FindIconInResources("Character")));
            //root.AddItem(new ProjectFolder(project, "Notes", IconHelper.FindIconInResources("Note")));

            //project.ProjectFiles.Add(root);

            //return project;
        }

        public ProjectModel SetUpTestProject(string header, string author, Forms form, Genres genre)
        {
            var project = new ProjectModel(header, author, form, genre, DateTime.Now, RootProjectsDirectory + $"\\{header}");

            this.ActiveProject = project;

            var root = new ProjectFolder(project.Name, IconHelper.FindIconInResources("Book")) { RootFolder = true, IsSelected = true };
            var locations = new ProjectFolder("Locations", IconHelper.FindIconInResources("Map"));

            root.AddItem(locations);

            locations.AddItem(new Location("Azeroth", IconHelper.FindIconInResources("Map"))
            {
                Name = "Azeroth",
                Description = "Azeroth is the name of the world in which the majority of the Warcraft series is set. At its core dwells a slumbering world-soul, " +
                "the nascent spirit of a titan. Long ago, it was invaded by the Old Gods, eldritch abominations from the Void. When the Pantheon arrived, " +
                "the titans imprisoned the Old Gods deep beneath the earth, before healing and ordering the world, and seeding life across the planet. " +
                "A great fount of magic that would nurture the land was placed at the center of Kalimdor, known as the Well of Eternity.",
                Tags = new ObservableCollection<Tag>() { new Tag("Big place"), new Tag("Full of nerds") }
            });

            locations.AddItem(new Location("Summoner's Rift", IconHelper.FindIconInResources("Map"))
            {
                Name = "Summoner's Rift",
                Description = "The Summoner's Rift is the most commonly used Field of Justice. The map was given a graphical and technical update on May 23rd, " +
                "2012 and remade from scratch on November 12th, 2014.",
                Tags = new ObservableCollection<Tag>() { new Tag("Washed up players"), new Tag("Nostalgic for S1") }
            });

            var characters = new ProjectFolder("Characters", IconHelper.FindIconInResources("Character"));

            root.AddItem(characters);

            characters.AddItem(new Character("Kaladin", IconHelper.FindIconInResources("Character"))
            {
                Name = "Kaladin",
                Short_Name = "Kal",
                Description = "An accomplished spearman and a natural leader, he eventually becomes the captain of Elhokar Kholin's King's Guard, " +
                "formerly known as the Cobalt Guard, House Kholin's personal honor guard.",
                Tags = new ObservableCollection<Tag>() { new Tag("Warrior"), new Tag("Main") },
                Goals = "Save the world, of course!"
            });

           characters.AddItem(new Character("Shallan", IconHelper.FindIconInResources("Character"))
            {
                Name = "Shallan",
                Short_Name = "Shal",
                Description = "Daughter of the recently deceased Brightlord Lin Davar of Jah Keved, Shallan pursued and received scholarly training as the ward of Jasnah Kholin.",
                Tags = new ObservableCollection<Tag>() { new Tag("Lightweaver"), new Tag("Main") },
               Goals = "Solve puzzles"
            });

            characters.AddItem(new Character("Jaskier", IconHelper.FindIconInResources("Character"))
            {
                Name = "Jaskier",
                Description = "Julian Alfred Pankratz, Viscount de Lettenhove, better known as Dandelion/Jaskier, was a poet, minstrel, bard, and close friend of Geralt of Rivia.",
                Tags = new ObservableCollection<Tag>() { new Tag("Poet"), new Tag("Drunk"), new Tag("Secondary") },
                Goals = "Get drunk and be an annoying stuck up cunt."
            });

            var notes = new ProjectFolder("Notes", IconHelper.FindIconInResources("Note"));

            root.AddItem(notes);

            var scenes = new ProjectFolder("Scenes", IconHelper.FindIconInResources("Pen"));

            scenes.AddItem(new Scene("Scene #1", IconHelper.FindIconInResources("Pen"))
            {
                Name = "Red Wedding",
                Description = "Starks get slaughtered like dogs"
            });

            scenes.AddItem(new Scene("Scene #2", IconHelper.FindIconInResources("Pen"))
            {
                Name = "Malfeasance",
                Description = "Kvothe gets expelled for causing harm to Ambrose"
            });

            scenes.AddItem(new Scene("Scene #3", IconHelper.FindIconInResources("Pen"))
            {
                Name = "K/DA",
                Description = "Riot Games single-handedly ruins No Nut November"
            });

            scenes.AddItem(new Scene("Scene #4", IconHelper.FindIconInResources("Pen"))
            {
                Name = "LoL Worlds 2018",
                Description = "\"It's the year of the West.\" Nice joke"
            });

            root.AddItem(scenes);

            project.AddSymbioticLink(new SymbioticLink<ProjectModel, ProjectFolder>(project, root));

            return project;
        }

        public ProjectModel SetUpPerformanceTestProject(string header, string author, Forms form, Genres genre)
        {
            var project = new ProjectModel(header, author, form, genre, DateTime.Now, RootProjectsDirectory + $"\\{header}");

            this.ActiveProject = project;

            var root = new ProjectFolder(project.Name, IconHelper.FindIconInResources("Book")) { RootFolder = true };

            var scenes = new ProjectFolder("Scenes", IconHelper.FindIconInResources("Pen"));

            root.AddItem(scenes);

            var locations = new ProjectFolder("Locations", IconHelper.FindIconInResources("Map"));

            root.AddItem(locations);

            var characters = new ProjectFolder("Characters", IconHelper.FindIconInResources("Character"));

            root.AddItem(characters);

            for (int i = 0; i < 100; i++)
            {
                var location = new Location(FieldGenerator.Instance.GenerateRandomCharacterName(), IconHelper.FindIconInResources("Map"));

                location.Name = FieldGenerator.Instance.GenerateRandomCharacterName();

                location.Description = FieldGenerator.Instance.GenerateRandomDescription();

                location.Details = FieldGenerator.Instance.GenerateRandomDescription();

                locations.AddItem(location);

                var character = new Character(FieldGenerator.Instance.GenerateRandomCharacterName(), IconHelper.FindIconInResources("Character"));

                character.Name = FieldGenerator.Instance.GenerateRandomCharacterName();

                character.Description = FieldGenerator.Instance.GenerateRandomDescription();

                character.Goals = FieldGenerator.Instance.GenerateRandomLine();

                characters.AddItem(character);

                var scene = new Scene(FieldGenerator.Instance.GenerateRandomCharacterName(), IconHelper.FindIconInResources("Pen"));

                scene.Description = FieldGenerator.Instance.GenerateRandomDescription();

                scene.Outcome = FieldGenerator.Instance.GenerateRandomLine();

                scene.Smells = FieldGenerator.Instance.GenerateRandomLine();

                scene.Sights = FieldGenerator.Instance.GenerateRandomLine();

                scene.Sounds = FieldGenerator.Instance.GenerateRandomLine();

                scene.AddItem(location);
                scene.AddItem(character);

                scenes.AddItem(scene);
            }

            project.AddSymbioticLink(new SymbioticLink<ProjectModel, ProjectFolder>(project, root));

            return project;
        }

        public void Delete_Project(ProjectModel project)
        {
            if (Directory.Exists(project.ProjectDirectory))
                Directory.Delete(project.ProjectDirectory, true);

            Projects.Remove(project);
        }

        #endregion
    }
}
