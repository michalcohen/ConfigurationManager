using ConfigurationManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.ViewModel
{
    public class FolderViewModel
    {
        public Folder RootFolder { get; set; }
        public FolderViewModel()
        {
            string base_path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            RootFolder = new Folder(base_path, is_expendedable: true, are_sub_folders_expendable: true);
        }
    }
}
