using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace ConfigurationManager.Model
{
    /// <summary>
    /// Represents any folders hierarchy in the disk (not neccacery configuration folders).
    /// Enables the user to pick a new configuration folder to open. If the configuration folder
    /// contains files with unrecognized extention, they won't be shown.
    /// </summary>
    public class Folder : INotifyPropertyChanged
    {
        /// <summary>
        /// The name of the file or folder (no full path) and the extention.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full path to the file or folder
        /// </summary>
        public string FullPath { get; set; }
        
        /// <summary>
        /// All sub folders
        /// </summary>
        public List<Folder> SubFolders {get; set;}

        public event PropertyChangedEventHandler PropertyChanged;

        public Folder(string path, bool is_expendedable = false, bool are_sub_folders_expendable = false)
        {
            FullPath = path;
            Name = path.Split("\\")[^1];
            SubFolders = new List<Folder>();
            if (is_expendedable)
            {
                DirSearch(are_sub_folders_expendable);
            }
            
        }

        /// <summary>
        /// Recursievly creates the folder hierarchy from a given path.
        /// </summary>
        private void DirSearch(bool are_sub_folders_expendable)
        {
            SubFolders = new List<Folder>();
            foreach (string d in Directory.GetDirectories(FullPath))
            {
                try
                {
                    SubFolders.Add(new Folder(d, is_expendedable: are_sub_folders_expendable));
                }
                catch (UnauthorizedAccessException)
                {
                }

            }
        }

        public void AddLayer()
        {
            foreach (Folder folder in SubFolders)
            {
                folder.DirSearch(true);
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
