using ConfigurationManager.Model;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
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
    /// Interaction logic for ProjectExplorer.xaml
    /// </summary>
    public partial class ProjectExplorer : UserControl
    {
        public ProjectExplorer()
        {
            InitializeComponent();
        }

        public void ShowFile(object sender, MouseButtonEventArgs args)
        {
            
            //((TextBlock)sender).Background = Brushes.ForestGreen;
            if (args.LeftButton == MouseButtonState.Pressed && args.ClickCount == 2)
            {
                if (sender is TextBlock)
                {
                    ((ProjectExplorerItem)((TextBlock)sender).DataContext).OpenTab();
                }
            }
            
        }

        
        private void TreeViewItem_MouseEnter(object sender, MouseEventArgs e)
        {/*
            if ((sender as TextBlock).DataContext.Equals(configuration_folder_view.SelectedItem))
            {
                ((TextBlock)sender).Background = Brushes.LightGreen;
            }
            else
            {
                ((TextBlock)sender).Background = Brushes.Aquamarine;
            }*/
            
        }

        private void TreeViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            //((TextBlock)sender).Background = Brushes.White;

        }
    }
}
