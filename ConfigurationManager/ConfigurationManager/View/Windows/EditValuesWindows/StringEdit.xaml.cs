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
        ConfigurationString CS;
        public StringEdit(ConfigurationString cs)
        {
            CS = cs;
            DataContext = cs;
            InitializeComponent();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            CS.ConfigurationName = nameText.Text;
            CS.Update(valeText.Text);
            Close();
        }
    }
}
