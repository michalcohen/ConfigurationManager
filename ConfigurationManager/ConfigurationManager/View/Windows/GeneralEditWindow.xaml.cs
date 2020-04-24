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

namespace ConfigurationManager.View.Windows
{
    /// <summary>
    /// Interaction logic for GeneralEditWindow.xaml
    /// </summary>
    public partial class GeneralEditWindow : Window
    {
        public GeneralEditWindow(ConfigurationVariable cv)
        {
            DataContext = cv;
            InitializeComponent();
        }

        private void edit_control_Loaded(object sender, RoutedEventArgs e)
        {
            edit_control.DataContext = (DataContext as ConfigurationVariable).Clone();
            edit_control.original_CV = DataContext as ConfigurationVariable;

        }
    }
}
