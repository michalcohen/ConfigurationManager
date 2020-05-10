using ConfigurationManager.Model;
using ConfigurationManager.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConfigurationManager
{
    /// <summary>
    /// Interaction logic for ChooseConfigurationFolder.xaml
    /// </summary>
    public partial class ChooseConfigurationFolder : Window
    {
        public ChooseConfigurationFolder(FolderViewModel fvm)
        {
            DataContext = fvm.RootFolder;
            InitializeComponent();
        }


        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void TreeViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            //((TextBlock)sender).Background = Brushes.Aquamarine;
        }

        private void TreeViewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            //((TextBlock)sender).Background = Brushes.White;
        }

        public string Answer
        {
            get { return ((Folder)folder_view.SelectedItem).FullPath; }
        }

        private void HierarchicalDataTemplate_Expanded(object sender, RoutedEventArgs e)
        {
            ((e.OriginalSource as TreeViewItem).DataContext as Folder).AddLayer();
        }
    }
}
