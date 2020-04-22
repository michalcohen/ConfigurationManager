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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConfigurationManager.View.UserControls.EditValuesWindows
{
    /// <summary>
    /// Interaction logic for EditFloat.xaml
    /// </summary>
    public partial class EditFloat : UserControl, EditUSerControl
    {
        ConfigurationFloat CF { get; set; }

        public EditFloat(ConfigurationFloat cf)
        {
            CF = cf;
            DataContext = cf;
            InitializeComponent();
        }

        public void SaveClick()
        {
            CF.Update(float.Parse(valueText.Text), float.Parse(lowestText.Text),
                float.Parse(highestText.Text));
        }
    }
}
