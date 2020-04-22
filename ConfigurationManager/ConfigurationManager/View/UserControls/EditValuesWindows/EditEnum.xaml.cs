﻿using ConfigurationManager.Model.Types;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConfigurationManager.View.UserControls.EditValuesWindows
{
    /// <summary>
    /// Interaction logic for EditEnum.xaml
    /// </summary>
    public partial class EditEnum : UserControl
    {
        private static string default_string = "Add option...";

        ConfigurationEnumeration CE;
        public EditEnum(ConfigurationEnumeration ce)
        {
            CE = ce;
            DataContext = ce;
            InitializeComponent();
        }


        private void add_enum_value_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (add_enum_value.Text.Equals(default_string))
            {
                add_enum_value.Text = "";
            }
        }

        private void add_local_enum_option(object sender)
        {
            ((sender as TextBox).DataContext as ConfigurationEnumeration).Value.Options.Add(add_enum_value.Text);
        }

        private bool add_enum_value_has_value()
        {
            return !add_enum_value.Text.Equals(default_string) && !add_enum_value.Text.Equals("");
        }

        private void add_enum_value_LostFocus(object sender, RoutedEventArgs e)
        {
            if (add_enum_value_has_value())
            {
                add_local_enum_option(sender);
            }
        }

        private void add_enum_value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter) && add_enum_value_has_value())
            {
                add_local_enum_option(sender);
                add_enum_value.Text = default_string;
            }
        }

        private void local_enum_option_delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
