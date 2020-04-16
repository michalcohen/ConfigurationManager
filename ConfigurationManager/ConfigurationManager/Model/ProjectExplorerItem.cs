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
        /// <summary>
        /// The location on the disk of this configuration file
        /// </summary>
        public string FullPath { get; set; }
        
        /// <summary>
        /// The text that appears on tree views that wish two show all the files in the configuration folder.
        /// for example "C:\Users\Bloop\Configuration\Threshold\weights.json" will be represented as "weights.json"
        /// </summary>
        public string ProjectExplorerItemsLabel { get; set; }

        /// <summary>
        /// This property is handy for binding to usercontrols that their data context is the entire ProjectExplorerItem
        /// </summary>
        public ProjectExplorerItem AutoReference { get; set; }

        /// <summary>
        /// If this Project explorer item represents folder, then this property contains all the subfolders and files.
        /// If this Project explorer item represents file, then this list is empty.
        /// </summary>
        public List<ProjectExplorerItem> ProjectExplorerItems { get; set; }

        /// <summary>
        /// The representation of the content of the file (the tree of configuration variables).
        /// </summary>
        public ConfigurationFile Content { get; set; }


        private bool is_opened;

        /// <summary>
        /// The index of tab containing this configuration file.
        /// </summary>
        public int tab;

        /// <summary>
        /// True <=> there is an opened tab containing this configuration file.
        /// </summary>
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

        /// <summary>
        /// True <=> any of the configuration variables in this file has been changed and not yet saved.
        /// </summary>
        public bool IsDirty { 
            get
            {
                return Content != null && Content.IsDirty;
            } 
        }

        /// <summary>
        /// Parents that need notification on changes in inner properties (such as ProjectModel).
        /// </summary>
        public Changable Father;

        /// <summary>
        /// Direct reference to the ProjectModel to save passing global data through the hierarchy of ProjectExplorerItems
        /// For example, if we want to know how many opened tabs exist, ask PM.
        /// </summary>
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

        /// <summary>
        /// Write content of the file to the disk if it is dirty,
        /// If this is a filder, save all its subfolders and file.
        /// </summary>
        internal void Save()
        {
            if (IsDirty)
            {
                System.IO.File.WriteAllText(FullPath, JsonConvert.SerializeObject(Content.GetDictionary(), Formatting.Indented));
                Content.Saved();
            }
            foreach (ProjectExplorerItem pei in ProjectExplorerItems)
            {
                pei.Save();
            }
        }

        /// <summary>
        /// Assign opened tab for this element.
        /// </summary>
        public void OpenTab()
        {
            if (!IsFile(FullPath))
            {
                return;
            }
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

        /// <summary>
        /// update tab index about this tab closing
        /// </summary>
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

        /// <summary>
        /// True <=> this is a representation of file and not directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsFile(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            return (attr & FileAttributes.Directory) != FileAttributes.Directory;
        }

        /// <summary>
        /// recurcievly build the tree structure that represents the configuration folder.
        /// </summary>
        /// <param name="sDir"></param>
        /// <param name="sTreeSons"></param>
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
