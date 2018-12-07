namespace Scribble.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SceneView.xaml
    /// </summary>
    public partial class SceneView : UserControl
    {
        public SceneView()
        {
            InitializeComponent();


            //Redraws the ExtendedRichTextBox user controls by switching tabs after exiting fullscreen. Temporary, until I come up with better solution.
            rtb.OnFullscreenToggled += (s, e) => 
            {
                if (!rtb.Fullscreen)
                {
                    tc.SelectedIndex = 1;
                    tc.SelectedIndex = 0;
                }
            };
        }
    }
}
