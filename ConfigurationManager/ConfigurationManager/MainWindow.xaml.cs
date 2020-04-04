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

namespace ConfigurationManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private readonly MyViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            ConfigurationTreeViewItem c = new ConfigurationTreeViewItem("Configuration");
            configuration_folder_view.Items.Add(c);
            DirSearch("Configurations", c);
            Enums.LoadEnums();
            LoadeJsons();
        }

        static void DirSearch(string sDir, ConfigurationTreeViewItem sTree)
        {
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
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


        private void LoadeJsons()
        {

            foreach (string file in Directory.GetFiles("Configurations", "*", SearchOption.AllDirectories))
            {
                if (file.Equals("Configurations\\Enums.json")){
                    continue;
                }
                using StreamReader r = new StreamReader(file);
                string json = r.ReadToEnd();
                JObject array = (JObject)JsonConvert.DeserializeObject(json);
                ConfigurationFile c = new ConfigurationFile(array);
                configuration_folder_view.Items.Add(new TreeViewItem());
                // @Lidor: here we will use the object c to create the GUI
            }
        }

    }
}
