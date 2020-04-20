using ConfigurationManager.Model.Types;
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

namespace ConfigurationManager.View.Windows
{
    /// <summary>
    /// Interaction logic for EditConfigurationVariable.xaml
    /// </summary>
    public partial class EditConfigurationVariable : Window
    {
        ConfigurationVariable CV;
        public EditConfigurationVariable(ConfigurationVariable cv)
        {
            CV = cv;
            DataContext = cv;
            InitializeComponent();

        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            CV.ConfigurationName = nameText.Text;
            CV.IsExplicit = IsExplicit.IsChecked.Value;
            (ContentControl.Content as EditUSerControl).SaveClick();
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
