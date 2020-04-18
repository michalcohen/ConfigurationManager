using ConfigurationManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConfigurationManager.View.Windows
{
    /// <summary>
    /// Interaction logic for EnumsView.xaml
    /// </summary>
    public partial class EnumsView : Window
    {
        public EnumsView(GlobalEnums ge)
        {
            GlobalEnums GE = ge;
            DataContext = ge;
            InitializeComponent();
        }

        private static string default_string = "Add value...";

        private void AddEnumOption(object sender)
        {
            TextBox tb = sender as TextBox;
            string content = tb.Text;
            if (!content.Equals("") && !content.Equals(default_string))
            {
                (tb.DataContext as GlobalEnum).AddOption(content);
                UpdateLayout();
            }
            tb.Text = default_string;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AddEnumOption(sender);
            (sender as TextBox).Foreground = Brushes.DimGray;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && !((sender as TextBox).Text.Equals("") || (sender as TextBox).Text.Equals(default_string)))
            {
                AddEnumOption(sender);
                (sender as TextBox).Text = "";
            } else if ((sender as TextBox).Text.Equals("") || (sender as TextBox).Text.Equals(default_string))
            {
                (sender as TextBox).Text = "";
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
            (sender as TextBox).Foreground = Brushes.Black;
        }
    }
}
