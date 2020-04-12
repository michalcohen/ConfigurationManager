using ConfigurationManager.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ConfigurationManager.GUIComponents
{
    class ConfigurationTreeItem: TreeViewItem
    {
        string ItemName { get; set; }
        ConfigurationVariable Model { get; set; }

        public ConfigurationTreeItem(string name, ConfigurationVariable model)
        {
            ItemName = name;
            Model = model;
            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
            sp.Children.Add(new Label() { Content = name.Equals("") ? "" : name + ": "});
            sp.Children.Add(new Label() { Content = model.ToString(), Foreground = model.GetFontColor() });
            Header = sp;
            IsExpanded = true;
        }
    }
}
