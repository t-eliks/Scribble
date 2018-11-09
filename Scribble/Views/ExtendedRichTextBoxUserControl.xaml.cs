namespace Scribble.Views
{
    using Scribble.Controls;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for ExtendedRichTextBoxUserControl.xaml
    /// </summary>
    public partial class ExtendedRichTextBoxUserControl : UserControl, INotifyPropertyChanged
    {
        public ExtendedRichTextBoxUserControl()
        {
            InitializeComponent();

            fontCB.ItemsSource = Fonts.SystemFontFamilies.OrderBy(x => x.Source);

            fontsizeCB.ItemsSource = new List<double>() { 8, 10, 12, 14, 16, 24, 36, 48, 72 };

            var r = rtb.Selection;

            if (r.Text == "")
            {
                fontCB.SelectedItem = new FontFamily("Times New Roman");
                rtb.FontFamily = new FontFamily("Times New Roman");
            }
            else
            {
                fontCB.SelectedItem = r.GetPropertyValue(FontFamilyProperty);
                rtb.FontFamily = (FontFamily)r.GetPropertyValue(FontFamilyProperty);
            }

            fontsizeCB.SelectedItem = r.GetPropertyValue(FontSizeProperty);

            rtb.SelectionChanged += (s, e) =>
            {
                r = rtb.Selection;

                if (r.GetPropertyValue(TextElement.FontWeightProperty).Equals(FontWeights.Normal))
                    boldbtn.IsChecked = false;
                else
                    boldbtn.IsChecked = true;

                if (r.GetPropertyValue(TextElement.FontStyleProperty).Equals(FontStyles.Normal))
                    italicbtn.IsChecked = false;
                else
                    italicbtn.IsChecked = true;

                fontCB.SelectedItem = r.GetPropertyValue(FontFamilyProperty);

                fontsizeCB.SelectedItem = r.GetPropertyValue(FontSizeProperty);
            };

            rtb.LostFocus += (s, e) => { e.Handled = true; };
        }

        private void Save_Changes(object sender, RoutedEventArgs e)
        {
            ExtendedRichTextBox.Save(rtb);
        }

        private void Bold_Selection(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                EditingCommands.ToggleBold.Execute(null, rtb);
                rtb.Focus();
            }
        }

        private void Italic_Selection(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                EditingCommands.ToggleItalic.Execute(null, rtb);
                rtb.Focus();
            }
        }

        private void Font_Changed(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                var r = rtb.Selection;

                if ((FontFamily)((ComboBox)sender).SelectedItem != null)
                    r.ApplyPropertyValue(FontFamilyProperty, (FontFamily)((ComboBox)sender).SelectedItem);

                rtb.Focus();
            }
        }

        private void FontSize_Changed(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                var r = rtb.Selection;

                if (((ComboBox)sender).SelectedItem != null)
                    r.ApplyPropertyValue(FontSizeProperty, (double)((ComboBox)sender).SelectedItem);

                rtb.Focus();
            }
        }

        private void Bullet_Selection(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                EditingCommands.ToggleBullets.Execute(null, rtb);
                rtb.Focus();
            }
        }

        private void Align_Left(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                EditingCommands.AlignLeft.Execute(null, rtb);
                rtb.Focus();
            }
        }

        private void Align_Center(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                EditingCommands.AlignCenter.Execute(null, rtb);
                rtb.Focus();
            }
        }

        private void Align_Right(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                EditingCommands.AlignRight.Execute(null, rtb);
                rtb.Focus();
            }
        }

        #region INotifyPropertyChanged Impl.

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }
}
