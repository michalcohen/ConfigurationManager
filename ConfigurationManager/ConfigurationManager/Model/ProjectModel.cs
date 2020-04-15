using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConfigurationManager.Model
{
    public class ProjectModel: INotifyPropertyChanged, Changable
    {
        public string RootPath { get; set; }
        public ProjectExplorerItem RootExplorerItem { get; set; }

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

        public int TabCount;

        public ProjectModel(string root_path, PropertyChangedEventHandler fatherPropertyChangedHandler)
        {
            RootPath = root_path;
            PropertyChanged += fatherPropertyChangedHandler;
            Enums.LoadEnums(RootPath);
            RootExplorerItem = new ProjectExplorerItem(RootPath, this, this);
            FocusedTab = -1;
        }

        internal List<ProjectExplorerItem> GetProjectExplorerOpenedItems()
        {
            return GetAllProjectExplorerItems().Where(pei => pei.IsOpened).ToList();

        }

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

        internal void Save()
        {
            RootExplorerItem.Save();
        }

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
