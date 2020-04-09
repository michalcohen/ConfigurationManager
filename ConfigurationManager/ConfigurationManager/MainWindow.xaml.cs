using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ConfigurationManager.GUIComponents;
using System.ComponentModel;

namespace ConfigurationManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly MyViewModel _viewModel;

        private static String RootPath;

        private ConfigurationModel model;

        private static readonly string configuration_manager_configuration = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\ConfiurationManagerData\\RecentConfigurationFolder.json";
        
        public MainWindow()
        {
            InitializeComponent();
            RootPath = GetRootPath();
            model = new ConfigurationModel(RootPath);
            CreateTreeViewOfConfiguraionFiles();
        }

        private void CreateTreeViewOfConfiguraionFiles()
        {
            ConfigurationTreeViewItem c = new ConfigurationTreeViewItem(RootPath);
            configuration_folder_view.Items.Add(c);
            DirSearch(RootPath, c);
        }

        private String GetRootPathFromDialog()
        {
            ChooseConfigurationFolder inputDialog = new ChooseConfigurationFolder();
            if (inputDialog.ShowDialog() == true)
                return inputDialog.Answer;
            return "";
        }

        private String GetRootPath()
        {
            if (!File.Exists(configuration_manager_configuration))
            {
                return GetRootPathFromDialog();
            } 
            using StreamReader r = new StreamReader(configuration_manager_configuration);
            string json = r.ReadToEnd();
            JObject array = (JObject)JsonConvert.DeserializeObject(json);
            string folder_name = array["recent_configuration_folder"].ToObject<string>();
            if (folder_name.Equals(""))
            {
                return GetRootPathFromDialog();
            }
            return folder_name;
        }

        static void DirSearch(string sDir, ConfigurationTreeViewItem sTree)
        {
            try
            {
                foreach (string f in Directory.GetFiles(sDir, "*.json"))
                {
                    sTree.Items.Add(new ConfigurationTreeViewItem(f));
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    ConfigurationTreeViewItem t = new ConfigurationTreeViewItem(d);
                    sTree.Items.Add(t);
                    DirSearch(d, t);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        void SaveContentIfNeeded()
        {
            if (!model.IsDirty())
            {
                return;
            }
            string messageBoxText = "Do you want to save changes?";
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            if (result != MessageBoxResult.Yes)
            {
                model.Save();
            }
        }

        void MainWindowClosing(object sender, EventArgs e)
        { 
            Dictionary<string, string> to_save = new Dictionary<string, string>();
            to_save["recent_configuration_folder"] = RootPath;
            string json = JsonConvert.SerializeObject(to_save);

            System.IO.FileInfo file = new System.IO.FileInfo(configuration_manager_configuration);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            System.IO.File.WriteAllText(file.FullName, json);
            SaveContentIfNeeded();
        }
        
        private void MenuExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuOpenClick(object sender, RoutedEventArgs e)
        {
            SaveContentIfNeeded();
            string root_path = GetRootPathFromDialog();
            if (root_path.Equals(""))
            {
                return;
            }
            RootPath = root_path;
            configuration_folder_view.Items.Clear();
            CreateTreeViewOfConfiguraionFiles();
            model = new ConfigurationModel(RootPath);
        }

        private void MenuSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                model.Save();
                MessageBox.Show("Configuration successfuly saved!");
            } catch (Exception)
            {
                MessageBox.Show("something went wrong while saving...");
            }
            
        }
    }
}
