using BrightIdeasSoftware;
using ConfigurationManager.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ConfigurationManager.GUIComponents
{
    public class ConfigurationFileContent : TreeView
    {
        public ConfigurationFileContent(ConfigurationFile f)
        {

            ConfigurationTreeItem cti = new ConfigurationTreeItem("content", f.Content);
            Items.Add(cti);
            foreach (KeyValuePair<string, ConfigurationVariable> son in f.Content.Variables)
            {
                LoadConfigurationVariable(son.Key, son.Value, cti);
            }
        }

        private void LoadConfigurationVariable(string name, ConfigurationVariable cv, ConfigurationTreeItem father)
        {
            if (cv is CompositeConfiguraionVariable)
            {
                ConfigurationTreeItem cti = new ConfigurationTreeItem(name, cv);
                father.Items.Add(cti);
                CompositeConfiguraionVariable ccv = cv as CompositeConfiguraionVariable;
                foreach (KeyValuePair<string, ConfigurationVariable> son in ccv.Variables)
                {
                    LoadConfigurationVariable(son.Key, son.Value, cti);
                }
            } else if  (cv is ConfigurationList)
            {
                ConfigurationTreeItem cti = new ConfigurationTreeItem(name, cv);
                father.Items.Add(cti);
                ConfigurationList ccv = cv as ConfigurationList;
                foreach (ConfigurationVariable son in ccv.Variables)
                {
                    LoadConfigurationVariable("", son, cti);
                }
            } else
            {
                father.Items.Add(new ConfigurationTreeItem(name, cv));
            }
        }
    }
}
