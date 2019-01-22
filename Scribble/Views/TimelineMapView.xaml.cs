using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scribble.Views
{
    /// <summary>
    /// Interaction logic for TimelineMapView.xaml
    /// </summary>
    public partial class TimelineMapView : UserControl
    {
        public TimelineMapView()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var itemscontrol = itm;
            var proxy = itm.FindResource("proxy");
        }
    }
}
