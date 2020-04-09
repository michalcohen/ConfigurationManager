using ConfigurationManager.GUIComponents;
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
        public ChooseConfigurationFolder()
        {
            InitializeComponent();
            string base_path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            ConfigurationTreeViewItem c = new ConfigurationTreeViewItem(base_path);
            folder_view.Items.Add(c);
            DirSearch(base_path, c);
        }


        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
        }

        public string Answer
        {
            get { return ((ConfigurationManager.GUIComponents.ConfigurationTreeViewItem)folder_view.SelectedItem).Path; }
        }

        static void DirSearch(string sDir, ConfigurationTreeViewItem sTree)
        {
            foreach (string d in Directory.GetDirectories(sDir))
            {
                if (d.Contains("."))
                {
                    continue;
                }
                try
                {
                    ConfigurationTreeViewItem t = new ConfigurationTreeViewItem(d);
                    sTree.Items.Add(t);
                    DirSearch(d, t);
                } catch (UnauthorizedAccessException)
                {
                }
                
            }
        }
    }
}
