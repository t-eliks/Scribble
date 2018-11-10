﻿namespace Scribble.Controls
{
    using Scribble.Models;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    public class TagTextBox : TextBox
    {
        public TagTextBox()
        {
            this.TextChanged += (s, e) =>
            {
                if (this.Text.Contains(", "))
                {
                    Content.Add(new Models.Tag(this.Text.Trim(',', ' ')));
                    this.Text = String.Empty;
                }
            };
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(ObservableCollection<Tag>), typeof(TagTextBox), new FrameworkPropertyMetadata(new ObservableCollection<Tag>())
            {
                PropertyChangedCallback = (s, e) =>
                {
                    foreach (var item in (IEnumerable<Tag>)e.NewValue)
                        item.OnMarkedForRemoval += (o, a) => { ((TagTextBox)s).Content.Remove(item); };
                }
            });

        public ObservableCollection<Tag> Content
        {
            get { return (ObservableCollection<Tag>)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
    }
}
