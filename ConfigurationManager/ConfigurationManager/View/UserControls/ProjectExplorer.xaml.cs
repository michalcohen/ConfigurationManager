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
            if (sender is TreeViewItem)
            {
                TreeViewItem ctvi = (((TreeViewItem)sender).IsSelected || configuration_folder_view.SelectedItem == null? sender : configuration_folder_view.SelectedItem) as TreeViewItem;
                ProjectExplorerItem cpei = ctvi.DataContext as ProjectExplorerItem;
                string path_to_add = cpei.FullPath;
                if (!path_to_add.Split(".")[^1].Equals("json"))
                {
                    return;
                }
                cpei.OpenTab();
            }
        }

        private void TreeViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            ((TreeViewItem)sender).Background = Brushes.Aquamarine;
        }

        private void TreeViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TreeViewItem)sender).Background = Brushes.White;
        }
    }
}
