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

        private static string default_value_string = "Add value...";
        private static string default_enum_string = "Add enum...";

        private void AddEnumOption(object sender)
        {
            TextBox tb = sender as TextBox;
            string content = tb.Text;
            if (!content.Equals("") && !content.Equals(default_value_string))
            {
                (tb.DataContext as GlobalEnum).AddOption(content);
                UpdateLayout();
            }
            tb.Text = default_value_string;
        }
        
        private void AddValue_LostFocus(object sender, RoutedEventArgs e)
        {
            AddEnumOption(sender);
            (sender as TextBox).Foreground = Brushes.DimGray;
        }

        private void AddValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && !((sender as TextBox).Text.Equals("") || (sender as TextBox).Text.Equals(default_value_string)))
            {
                AddEnumOption(sender);
                (sender as TextBox).Text = "";
            } else if ((sender as TextBox).Text.Equals("") || (sender as TextBox).Text.Equals(default_value_string))
            {
                (sender as TextBox).Text = "";
            }
        }

        private void AddValue_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
            (sender as TextBox).Foreground = Brushes.Black;
        }

        private void GlobalEnumValue_Delete(object sender, RoutedEventArgs e)
        {
            ((sender as MenuItem).DataContext as GlobalEnumValue).Delete();
        }

        private void GlobalEnum_Delete(object sender, RoutedEventArgs e)
        {
            ((sender as MenuItem).DataContext as GlobalEnum).Delete();
        }

        private void GlobalEnum_Edit(object sender, RoutedEventArgs e)
        {
            TextBox tb = (((sender as MenuItem).Parent as ContextMenu).PlacementTarget) as TextBox;
            tb.IsReadOnly = false;
            tb.Focus();
            tb.CaretIndex = 0;
        }

        private void EditGlobalEnum_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).IsReadOnly = true;
        }

        private void AddEnum(object sender)
        {
            TextBox tb = sender as TextBox;
            string content = tb.Text;
            if (!content.Equals("") && !content.Equals(default_enum_string))
            {
                GlobalEnums.GetIntance().AddEnum(content);
                UpdateLayout();
            }
            tb.Text = default_enum_string;
        }


        private void AddEnum_LostFocus(object sender, RoutedEventArgs e)
        {
            AddEnum(sender);
            (sender as TextBox).Foreground = Brushes.DimGray;
        }

        private void AddEnum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && !((sender as TextBox).Text.Equals("") || (sender as TextBox).Text.Equals(default_enum_string)))
            {
                AddEnum(sender);
                (sender as TextBox).Text = "";
            }
            else if ((sender as TextBox).Text.Equals("") || (sender as TextBox).Text.Equals(default_value_string))
            {
                (sender as TextBox).Text = "";
            }
        }

        private void AddEnum_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Text = "";
            (sender as TextBox).Foreground = Brushes.Black;
        }

    }
}
