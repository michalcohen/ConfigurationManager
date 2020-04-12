using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ConfigurationManager.GUIComponents
{
    class ActionTabItem
    {
        public string Header { get; set; }
        // This will be the content of the tab control It is a UserControl whits you need to create manualy
        public TreeView Content { get; set; }

        public string FileName { get; set; }

        public bool IsVisible { get; set; }
    }
}
