using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ConfigurationManager.ViewModel;
using ConfigurationManager.Model;
using ConfigurationManager.View.Windows;

namespace ConfigurationManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static String RootPath;

        private ProjectViewModel PVM;

        private bool IsUderTest = false;

        private static readonly string configuration_manager_configuration = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\ConfiurationManagerData\\RecentConfigurationFolder.json";

        public MainWindow(bool is_under_test)
        {
            InitializeComponent();
            if (is_under_test)
            {
                RootPath = @"D:\ConfigurationManager\ConfigurationManager\ConfigurationManager\ConfigurationManager\bin\Debug\netcoreapp3.1\Resources\ConfigurationsForExample";
                IsUderTest = true;
            } else
            {
                RootPath = GetRootPath();
            }
            PVM = new ProjectViewModel(RootPath);
        }

        private String GetRootPathFromDialog()
        {
            ChooseConfigurationFolder inputDialog = new ChooseConfigurationFolder(new FolderViewModel());
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

        void SaveContentIfNeeded()
        {
            if (!PVM.IsDirty())
            {
                return;
            }
            string messageBoxText = "Do you want to save changes?";
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    PVM.Save();
                    MessageBox.Show("Configuration successfuly saved!");
                }
                catch (Exception)
                {
                    MessageBox.Show("something went wrong while saving...");
                }
            }
        }

        void MainWindowClosing(object sender, EventArgs e)
        { 
            if (!IsUderTest)
            {
                Dictionary<string, string> to_save = new Dictionary<string, string>();
                to_save["recent_configuration_folder"] = RootPath;
                string json = JsonConvert.SerializeObject(to_save);
                System.IO.FileInfo file = new System.IO.FileInfo(configuration_manager_configuration);
                file.Directory.Create(); // If the directory already exists, this method does nothing.
                System.IO.File.WriteAllText(file.FullName, json);
            }
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
            PVM = new ProjectViewModel(RootPath);
            ProjectExplorerControl.DataContext = PVM.PM;
            ProjectExplorerControl.UpdateLayout();
            TabsControl.DataContext = PVM.PM;
            TabsControl.UpdateLayout();
        }

        private void MenuSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PVM.IsDirty())
                {
                    PVM.Save();
                    MessageBox.Show("Configuration successfuly saved!");
                } else
                {
                    MessageBox.Show("No changes detected!");
                }
                
                
            } catch (Exception)
            {
                MessageBox.Show("something went wrong while saving...");
            }
            
        }
  
        private void ProjectExplorerControl_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectExplorerControl.DataContext = PVM.PM;
        }
        
        private void TabsControl_Loaded(object sender, RoutedEventArgs e)
        {
            TabsControl.DataContext = PVM.PM;
        }

        private void MenuEnums_Click(object sender, RoutedEventArgs e)
        {
            (new EnumsView(GlobalEnums.GetIntance())).Show();
        }
    }
}
