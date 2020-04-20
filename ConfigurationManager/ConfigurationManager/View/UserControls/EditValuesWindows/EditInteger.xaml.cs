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
    /// Interaction logic for EditInteger.xaml
    /// </summary>
    public partial class EditInteger : UserControl, EditUSerControl
    {
        ConfigurationInteger CI { get; set; }

        public EditInteger(ConfigurationInteger ci)
        {
            CI = ci;
            DataContext = ci;
            InitializeComponent();
        }

        public void SaveClick()
        {
            CI.Update(int.Parse(valueText.Text), int.Parse(lowestText.Text),
                int.Parse(highestText.Text));
        }
    }
}