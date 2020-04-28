﻿using ConfigurationManager.Model.Types;
using ConfigurationManager.View.UserControls.EditValuesWindows;
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

namespace ConfigurationManager.View.UserControls.EditValuesWindows
{
    /// <summary>
    /// Interaction logic for EditConfigurationVariable.xaml
    /// </summary>
    public partial class EditConfigurationVariable : UserControl
    {
        public ConfigurationVariable original_CV;
        public EditConfigurationVariable()
        {
            InitializeComponent();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (original_CV == null)
            {
                original_CV = DataContext as ConfigurationVariable;
            } else
            {
                original_CV.UpdateBy(DataContext as ConfigurationVariable);
            }
            Window.GetWindow(this).Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}