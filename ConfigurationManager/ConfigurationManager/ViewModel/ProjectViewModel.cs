using ConfigurationManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace ConfigurationManager.ViewModel
{
    public class ProjectViewModel: INotifyPropertyChanged
    {
        public ProjectModel PM { get; set; }

        public ProjectViewModel(string root_path)
        {
            PM = new ProjectModel(root_path, PropertyChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public bool IsDirty()
        {
            return PM.GetAllProjectExplorerItems().Any(pei => pei.IsDirty);
        }

        internal void Save()
        {
            PM.Save();
        }
    }

    
}
