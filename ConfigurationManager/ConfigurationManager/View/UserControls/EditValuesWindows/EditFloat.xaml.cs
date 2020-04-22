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
    /// Interaction logic for EditFloat.xaml
    /// </summary>
    public partial class EditFloat : UserControl
    {
        public EditFloat(ConfigurationFloat cf)
        {
            DataContext = cf;
            InitializeComponent();
        }

    }
}
