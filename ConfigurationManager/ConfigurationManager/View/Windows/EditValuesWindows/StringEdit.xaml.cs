using ConfigurationManager.Model.Types;
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

namespace ConfigurationManager.GUIComponents.EditValuesWindows
{
    /// <summary>
    /// Interaction logic for StringEdit.xaml
    /// </summary>
    public partial class StringEdit : Window
    {

        class ViewModel
        {
            ConfigurationString CS;
            string VariableName;

            public ViewModel(ConfigurationString cs, string variable_name)
            {
                CS = cs;
                VariableName = variable_name;
            }
        }
        ViewModel vm;
        public StringEdit(ConfigurationString cs, string variable_name)
        {
            vm = new ViewModel(cs, variable_name);
            InitializeComponent();
            DataContext = vm;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
