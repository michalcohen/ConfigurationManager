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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConfigurationManager.View.UserControls
{
    /// <summary>
    /// Interaction logic for ConfigurationFileContent.xaml
    /// </summary>
    public partial class ConfigurationFileContent : UserControl
    {

        public ProjectExplorerItem Context;

        public ConfigurationFileContent()
        {
            InitializeComponent();
        }

        private void TreeViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
            ((TreeViewItem)sender).Background = Brushes.Aquamarine;
        }

        private void TreeViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            ((TreeViewItem)sender).Background = Brushes.White;
        }

        private void TreeViewItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (((TreeViewItem)sender).DataContext as ConfigurationVariable).OpenEditWindow();
        }
    }
}
