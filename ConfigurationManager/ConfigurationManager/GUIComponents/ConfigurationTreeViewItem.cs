using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ConfigurationManager.GUIComponents
{
    class ConfigurationTreeViewItem: TreeViewItem
    {
        public String Path { get; }

        public ConfigurationTreeViewItem(string path)
        {
            Header = path.Split("\\")[^1];
            Path = path;

        }
    }
}
