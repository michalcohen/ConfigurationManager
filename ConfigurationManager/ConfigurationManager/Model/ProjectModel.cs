using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConfigurationManager.Model
{
    /// <summary>
    /// Holds all the data needed for the project. This data will be binded to the views
    /// </summary>
    public class ProjectModel: INotifyPropertyChanged, Changable
    {
        /// <summary>
        /// The path of the root folder of the configurations.
        /// If the user has already opened in the past some configuration folder, it will be opened.
        /// If the user has never opened project before, then "Choos configuration folder" window will be opened.
        /// the user can always change the root path by clicking on file -> open.
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// The known global enums given in "enums.json"
        /// </summary>
        public GlobalEnums GE { get; set; }

        /// <summary>
        /// The root ProjectExplorerItem. each item describes eather subfolder of the root folder or configuration file.
        /// Note: this is the root of the tree representing the data!
        /// </summary>
        public ProjectExplorerItem RootExplorerItem { get; set; }

        /// <summary>
        /// The tab that is focused on. When file is opened for editing or when the user clicks different tab,
        /// the focused tab will be changed.
        /// </summary>
        private int focused_tab;
        public int FocusedTab
        {
            get
            {
                return focused_tab;
            }
            set
            {
                if (value != focused_tab)
                {
                    focused_tab = value;
                    RaisePropertyChanged("FocusedTab");
                }
            }
        }

        /// <summary>
        /// The total amount of tabs. Since newly opened files will be opened on last tab, this counter
        /// enables us to give these new windows the correct tab index for later use.
        /// </summary>
        public int TabCount;

        public ProjectModel(string root_path)
        {
            RootPath = root_path;
            GlobalEnums.LoadEnums(RootPath);
            GE = GlobalEnums.GetIntance();
            RootExplorerItem = new ProjectExplorerItem(RootPath, this, this);
            FocusedTab = -1;
        }


        /// <summary>
        /// Returns all the project explorer items (i.e. files and folders) in a single list instead of the
        /// tree structure they are stored in.
        /// </summary>
        /// <returns></returns>
        public List<ProjectExplorerItem> GetAllProjectExplorerItems()
        {
            return AuxGetAllProjectExplorerItems(RootExplorerItem);
        }

        private List<ProjectExplorerItem> AuxGetAllProjectExplorerItems(ProjectExplorerItem curr)
        {
            if (curr.ProjectExplorerItems.Count > 0)
            {
                List<ProjectExplorerItem> l = new List<ProjectExplorerItem>();
                foreach (ProjectExplorerItem p in curr.ProjectExplorerItems)
                {
                    l.AddRange(AuxGetAllProjectExplorerItems(p));
                }
                return l;
            }
            return new List<ProjectExplorerItem>() { curr };
        }

        /// <summary>
        /// Each file will save its content to the disk, and each configuration variable property "Dirty"
        /// will be set with "false", so that aditional saving will be suggested.
        /// </summary>
        internal void Save()
        {
            RootExplorerItem.Save();
            GlobalEnums.Save(RootPath);
        }

        /// <summary>
        /// Return a list of all the files that are opened to view in the tab control section.
        /// </summary>
        /// <returns></returns>
        public List<ProjectExplorerItem> OpenedExplorerItems
        {
            get
            {
                List<ProjectExplorerItem> l = GetAllProjectExplorerItems().Where(pei => pei.IsOpened).ToList();
                l.Sort((x, y) => x.tab - y.tab);
                return l;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// One of my project explorer item children wanted to notice me on a change.
        /// This behavior supports binding of ProjectModule property that is affected by inner object property
        /// change, such as ProjectExplorerItem or ConfigurationVariable properties.
        /// </summary>
        /// <param name="property"></param>
        public void Changed(string property)
        {
            if (property.StartsWith("IsOpened"))
            {
                int tab_closed = int.Parse(property.Split("$")[^1]);
                foreach (ProjectExplorerItem pei in GetAllProjectExplorerItems())
                {
                    if (pei.tab > tab_closed)
                    {
                        pei.tab--;
                    }
                }
                RaisePropertyChanged("OpenedExplorerItems");
            }
            RaisePropertyChanged(property);
        }
    }
}
