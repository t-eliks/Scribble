namespace Scribble.Views
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for TimelineUC.xaml
    /// </summary>
    public partial class TimelineUC : UserControl
    {
        public TimelineUC()
        {
            InitializeComponent();

            addrowBtn.MouseEnter += (o, a) => { s.Text = "Add row"; };
            addrowBtn.MouseLeave += (o, a) => { s.Text = ""; };
            subtractrowBtn.MouseEnter += (o, a) => { s.Text = "Subtract row"; };
            subtractrowBtn.MouseLeave += (o, a) => { s.Text = ""; };
            addSceneBtn.MouseEnter += (o, a) => { s.Text = "Add a scene"; };
            addSceneBtn.MouseLeave += (o, a) => { s.Text = ""; };
            removalBtn.MouseEnter += (o, a) => { s.Text = "Remove timeline"; };
            removalBtn.MouseLeave += (o, a) => { s.Text = ""; };
        }
    }
}
