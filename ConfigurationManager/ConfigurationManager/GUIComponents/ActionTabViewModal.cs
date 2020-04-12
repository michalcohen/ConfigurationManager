using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConfigurationManager.GUIComponents
{
    class ActionTabViewModal
    {
        public Dictionary<string, Tuple<ConfigurationFile, ActionTabItem>> opened_files;
        public ObservableCollection<ActionTabItem> Tabs { get; set; }

        public ActionTabViewModal()
        {
            opened_files = new Dictionary<string, Tuple<ConfigurationFile, ActionTabItem>>();
            Tabs = new ObservableCollection<ActionTabItem>();
        }

        public int AddOpenedFile(string name, ConfigurationFile model)
        {
            if (opened_files.ContainsKey(name))
            {
                return Tabs.IndexOf(opened_files[name].Item2);
            }
            ActionTabItem tab = new ActionTabItem { Header = name.Split("\\")[^1], Content = new ConfigurationFileContent(model), FileName = name };
            opened_files[name] = new Tuple<ConfigurationFile, ActionTabItem>(model, tab);
            Tabs.Add(tab);
            return Tabs.Count - 1;
        }

        public void RemoveOpenedFile(string name)
        {
            opened_files.Remove(name);
        }
    }
}
