using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace ConfigurationManager.Model
{
    public class Folder : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string FullPath { get; set; }

        public bool is_selected;
        public bool IsSelected
        {
            get { 
                return is_selected; 
            }
            set
            {
                if (value != is_selected)
                {
                    is_selected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }

        public List<Folder> SubFolders {get; set;}

        public event PropertyChangedEventHandler PropertyChanged;

        public Folder(string path)
        {
            FullPath = path;
            Name = path.Split("\\")[^1];
            SubFolders = new List<Folder>();
            DirSearch();
        }

        void DirSearch()
        {
            foreach (string d in Directory.GetDirectories(FullPath))
            {
                if (d.Contains("."))
                {
                    continue;
                }
                try
                {
                    Folder t = new Folder(d);
                    SubFolders.Add(t);
                }
                catch (UnauthorizedAccessException)
                {
                }

            }
        }

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
