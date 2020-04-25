using ConfigurationManager.Model;
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
    /// Interaction logic for AddValue.xaml
    /// </summary>
    public partial class AddValue : Window
    {
        ConfigurationVariable CV;
        public AddValue(ConfigurationVariable cv)
        {
            CV = cv;
            DataContext = new AddedValue(cv);
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (edit_control.original_CV != null)
            {
                CV.AddVariable(edit_control.original_CV as ConfigurationVariable);
            }
        }
    }
}
