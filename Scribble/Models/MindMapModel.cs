﻿namespace Scribble.Models
{
    using Scribble.Interfaces;
    using Scribble.Logic;
    using Scribble.ViewModels;
    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Windows.Input;

    [Serializable]
    public class MindMapModel : BaseItem, ISerializable, IViewItem, ITwoField
    {
        public MindMapModel(string name) : base(name, IconHelper.FindIconInResources("Mindmap"))
        {

        }

        public ObservableCollection<MindMapItemModel> MapContent
        {
            get
            {
                return ProjectService.Instance.FindLinks<MindMapItemModel>(this);
            }
        }

        private string _Description = "No description.";

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

        protected MindMapModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Description = info.GetString("description");
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("description", Description);
        }

    }
}
