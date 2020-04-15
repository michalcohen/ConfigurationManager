using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace ConfigurationManager.Model
{
    public class ProjectExplorerItem: INotifyPropertyChanged, Changable
    {
        public string FullPath { get; set; }


        public string AllNames
        {
            get
            {
                return Content.AllNames;
            }
        }
        public string ProjectExplorerItemsLabel { get; set; }

        public ProjectExplorerItem AutoReference { get; set; }

        public List<ProjectExplorerItem> ProjectExplorerItems { get; set; }

        public ConfigurationFile Content { get; set; }

        private bool is_opened;

        public int tab;
        public bool IsOpened {
            get
            {
                return is_opened;
            }
            set { if (is_opened != value)
                {
                    is_opened = value;
                    RaisePropertyChanged("OpenedExplorerItems");
                }
            } 
        }

        public bool IsDirty { 
            get
            {
                return Content != null && Content.IsDirty;
            } 
        }

        public Changable Father;

        public ProjectModel PM;

        public ProjectExplorerItem(string path, Changable father, ProjectModel pm)
        {
            FullPath = path;
            ProjectExplorerItemsLabel = GetFileName(path);
            Father = father;
            PM = pm;
            ProjectExplorerItems = new List<ProjectExplorerItem>();
            if (path.EndsWith(".json"))
            {
                using StreamReader r = new StreamReader(path);
                string json = r.ReadToEnd();
                JObject array = (JObject)JsonConvert.DeserializeObject(json);
                Content = new ConfigurationFile(array);
            }
            DirSearch(path, ProjectExplorerItems);
            IsOpened = false;
            AutoReference = this;
        }

        internal void Save()
        {
            System.IO.File.WriteAllText(FullPath, JsonConvert.SerializeObject(Content.GetDictionary(), Formatting.Indented));
            Content.Saved();
        }

        public void OpenTab()
        {
            if (!IsOpened)
            {
                tab = PM.TabCount;
                PM.TabCount++;
                IsOpened = true;
                PM.FocusedTab = tab;
                
            } else
            {
                PM.FocusedTab = tab;
            }
        }

        public void CloseTab()
        {
            if (IsOpened)
            {
                PM.TabCount--;
                IsOpened = false;
                RaisePropertyChanged("IsOpened$" + tab);
                tab = -1;
            }
        }

        private bool IsFile(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return (attr & FileAttributes.Directory) != FileAttributes.Directory;
        }

        private void DirSearch(string sDir, List<ProjectExplorerItem> sTreeSons)
        {
            try
            {
                if (IsFile(sDir)) {
                    return;
                }
                foreach (string f in Directory.GetFiles(sDir, "*.json"))
                {
                    if (f.Contains("\\Enums.json"))
                    {
                        continue;
                    }
                    sTreeSons.Add(new ProjectExplorerItem(f, this, PM));
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    if (!Directory.EnumerateFileSystemEntries(d).Any())
                    {
                        continue;
                    }
                    ProjectExplorerItem t = new ProjectExplorerItem(d, this, PM);
                    sTreeSons.Add(t);
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        private static string GetFileName(string path)
        {
            return path.Split("\\")[^1];
        }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void Changed(string property)
        {
            Father.Changed(property);
        }

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                Father.Changed(property);
            }
        }

    }
}
