﻿namespace Scribble.Controls
{
    using Scribble.Interfaces;
    using Scribble.Models;
    using Scribble.ViewModels;
    using Scribble.ViewModels.DialogViewModels;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;

    public class MindMapLine : UIElement, ITwoField
    {
        public MindMapLine(MindMapContent content1, MindMapContent content2)
        {
            try
            {
                ToolTipService.InitialShowDelayProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(0));
                ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
            }
            catch (ArgumentException ex)
            {
                
            }

            MindMapContent1 = content1;
            MindMapContent2 = content2;

            var linemodel = new MindMapLineModel(content1.Item, content2.Item);

            LineModel = linemodel;

            Name = linemodel.Header;
            Description = linemodel.Description;
        }

        public MindMapLine(MindMapContent content1, MindMapContent content2, MindMapLineModel linemodel)
        {
            try
            {
                ToolTipService.InitialShowDelayProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(0));
                ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
            }
            catch (ArgumentException ex)
            {

            }

            MindMapContent1 = content1;
            MindMapContent2 = content2;

            LineModel = linemodel;

            Name = linemodel.Header;
            Description = linemodel.Description;
            Color = linemodel.Color;
        }

        public MindMapCanvas ParentCanvas { get; set; }

        private MindMapLineModel LineModel { get; set; }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name",
          typeof(string), typeof(MindMapLine), new FrameworkPropertyMetadata(null) { PropertyChangedCallback = (s, e) => {
              var snd = (MindMapLine)s;
                snd.LineModel.Header = e.NewValue as string;
              if (snd.Line != null && snd.Line.ToolTip != null)
                    ((MindMapLineToolTip)snd.Line.ToolTip).Header = (string)e.NewValue;
              
          } });

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color",
          typeof(MindMapItemColors), typeof(MindMapLine), new FrameworkPropertyMetadata(null)
          {
              PropertyChangedCallback = (s, e) => {
                  var snd = (MindMapLine)s;
                  if (snd.Line != null)
                    snd.Line.Stroke = MindMapItemModel.GetBrush((MindMapItemColors)e.NewValue);
                  snd.LineModel.Color = (MindMapItemColors)e.NewValue;
              }
          });

        public MindMapItemColors Color
        {
            get { return (MindMapItemColors)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
          typeof(string), typeof(MindMapLine), new FrameworkPropertyMetadata(null) { PropertyChangedCallback = (s, e) => {
              var snd = (MindMapLine)s;
              snd.LineModel.Description = e.NewValue as string;
              if (snd.Line != null && snd.Line.ToolTip != null)
                  ((MindMapLineToolTip)snd.Line.ToolTip).Description = (string)e.NewValue;
          } });

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public MindMapContent MindMapContent1 { get; private set; }

        public MindMapContent MindMapContent2 { get; private set; }

        private Line Line { get; set; }

        public void RedrawLine()
        {
            if (Line != null && ParentCanvas.Children.Contains(Line))
                ParentCanvas.Children.Remove(Line);

            if (ParentCanvas != null && ParentCanvas.Children.Contains(MindMapContent1) && ParentCanvas.Children.Contains(MindMapContent2))
            {
                Point relativePoint = MindMapContent1.TransformToAncestor(ParentCanvas).Transform(new Point(0, 0));
                Point relativePoint2 = MindMapContent2.TransformToAncestor(ParentCanvas).Transform(new Point(0, 0));
                Point pt1 = new Point(relativePoint.X + MindMapContent1.ActualWidth / 2, relativePoint.Y + MindMapContent1.ActualHeight / 2);
                Point pt2 = new Point(relativePoint2.X + MindMapContent2.ActualWidth / 2, relativePoint2.Y + MindMapContent2.ActualHeight / 2);
                Line l = new Line();
                l.Stroke = MindMapItemModel.GetBrush(LineModel.Color);
                l.StrokeThickness = 5.0;
                l.X1 = pt1.X;
                l.X2 = pt2.X;
                l.Y1 = pt1.Y;
                l.Y2 = pt2.Y;

                l.ToolTip = new MindMapLineToolTip() { Description = Description, Header = Name };

                ContextMenu menu = new ContextMenu();

                MenuItem menuitem = new MenuItem() { Header = "Change Color", Template = App.Current.TryFindResource("SubmenuHeader") as ControlTemplate };

                menuitem.Items.Add(GetColorMenuItem("Charcoal", MindMapItemColors.Charcoal));
                menuitem.Items.Add(GetColorMenuItem("Embers", MindMapItemColors.Embers));
                menuitem.Items.Add(GetColorMenuItem("Ink", MindMapItemColors.Ink));
                menuitem.Items.Add(GetColorMenuItem("Jewel", MindMapItemColors.Jewel));
                menuitem.Items.Add(GetColorMenuItem("Teal", MindMapItemColors.Teal));
                menuitem.Items.Add(GetColorMenuItem("Vermillion", MindMapItemColors.Vermillion));
                menuitem.Items.Add(GetColorMenuItem("Void", MindMapItemColors.Void));
                menuitem.Items.Add(GetColorMenuItem("Watermelon", MindMapItemColors.Watermelon));

                MenuItem menuitem2 = new MenuItem() { Header = "Edit Line", Template = App.Current.TryFindResource("SubmenuItem") as ControlTemplate };
                menuitem2.Click += (o, a) =>
                {
                    var dialog = new TwoFieldInfoViewModel(true) { Item = this };

                    MainViewModel._DialogService.OpenDialog(dialog);
                };

                MenuItem menuitem3 = new MenuItem() { Header = "Remove", Template = App.Current.TryFindResource("SubmenuItem") as ControlTemplate };
                menuitem3.Click += (o, a) => 
                {
                    if (ParentCanvas.Children.Contains(Line))
                        ParentCanvas.Children.Remove(Line);

                    if (this.MindMapContent1.Lines.Contains(this))
                        this.MindMapContent1.Lines.Remove(this);

                    if (this.MindMapContent2.Lines.Contains(this))
                        this.MindMapContent2.Lines.Remove(this);

                    this.LineModel.Remove();
                };

                menu.Items.Add(menuitem);
                menu.Items.Add(menuitem2);
                menu.Items.Add(menuitem3);

                l.ContextMenu = menu;

                Line = l;

                ParentCanvas.Children.Add(l);
            }
        }

        private MenuItem GetColorMenuItem(string header, MindMapItemColors color)
        {
            MenuItem menuitem = new MenuItem() { Header = header, Template = App.Current.TryFindResource("SubmenuItem") as ControlTemplate };
            menuitem.Click += (o, a) => { Color = color; };

            return menuitem;
        }

        public bool IsLink(MindMapContent content1, MindMapContent content2)
        {
            return (content1 == MindMapContent1 && content2 == MindMapContent2) || (content2 == MindMapContent1 && content1 == MindMapContent2);
        }
    }
}
