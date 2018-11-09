namespace Scribble.Models
{
    using Scribble.Logic;
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Windows;
    using System.Windows.Media;

    [Serializable]
    public class Scene : ProjectFile, ISerializable
    {
        public Scene() { }

        public Scene(string name, ImageSource imageSource)
            : base(name, imageSource)
        {
            Description = "No description.";
        }

        private string _Role;

        public string Role
        {
            get
            {
                return _Role;
            }
            set
            {
                if (_Role != value)
                {
                    _Role = value;

                    RaisePropertyChanged(nameof(Role));
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

        private string _Sights;

        public string Sights
        {
            get
            {
                return _Sights;
            }
            set
            {
                if (_Sights != value)
                {
                    _Sights = value;

                    RaisePropertyChanged(nameof(Sights));
                }
            }
        }

        private string _Sounds;

        public string Sounds
        {
            get
            {
                return _Sounds;
            }
            set
            {
                if (_Sounds != value)
                {
                    _Sounds = value;

                    RaisePropertyChanged(nameof(Sounds));
                }
            }
        }

        private string _Smells;

        public string Smells
        {
            get
            {
                return _Smells;
            }
            set
            {
                if (_Smells != value)
                {
                    _Smells = value;

                    RaisePropertyChanged(nameof(Smells));
                }
            }
        }

        private string _Notes;

        public string Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                if (_Notes != value)
                {
                    _Notes = value;

                    RaisePropertyChanged(nameof(Notes));
                }
            }
        }

        private string _Outcome;

        public string Outcome
        {
            get
            {
                return _Outcome;
            }
            set
            {
                if (_Outcome != value)
                {
                    _Outcome = value;

                    RaisePropertyChanged(nameof(Outcome));
                }
            }
        }

        private string _Tags;

        public string Tags
        {
            get
            {
                return _Tags;
            }
            set
            {
                if (_Tags != value)
                {
                    _Tags = value;

                    RaisePropertyChanged(nameof(Tags));
                }
            }
        }

        private double _CanvasLeft = 0;

        public double CanvasLeft
        {
            get
            {
                return _CanvasLeft;
            }
            set
            {
                if (_CanvasLeft != value)
                {
                    _CanvasLeft = value;

                    RaisePropertyChanged(nameof(CanvasLeft));
                }
            }
        }

        private double _CanvasTop = 0;

        public double CanvasTop
        {
            get
            {
                return _CanvasTop;
            }
            set
            {
                if (_CanvasTop != value)
                {
                    _CanvasTop = value;

                    RaisePropertyChanged(nameof(CanvasTop));
                }
            }
        }

        public bool IsInTimeline { get; set; }

        public override string CreateFile()
        {
            return FileService.GenerateEmptyRtf(ProjectService.Instance.ActiveProject?.FileDirectory);
        }

        public override void Delete()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);

            base.Delete();
        }

        #region Serialization

        protected Scene(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Role = info.GetString("role");
            Description = info.GetString("description");
            Sights = info.GetString("sights");
            Sounds = info.GetString("sounds");
            Smells = info.GetString("smells");
            Tags = info.GetString("tags");
            Notes = info.GetString("notes");
            Outcome = info.GetString("outcome");
            CanvasLeft = info.GetDouble("canvasleft");
            CanvasTop = info.GetDouble("canvastop");
            IsInTimeline = info.GetBoolean("isintimeline");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("role", Role);
            info.AddValue("description", Description);
            info.AddValue("sights", Sights);
            info.AddValue("sounds", Sounds);
            info.AddValue("smells", Smells);
            info.AddValue("tags", Tags);
            info.AddValue("notes", Notes);
            info.AddValue("outcome", Outcome);
            info.AddValue("canvasleft", CanvasLeft);
            info.AddValue("canvastop", CanvasTop);
            info.AddValue("isintimeline", IsInTimeline);
        }

        #endregion

    }
}
