using ConfigurationManager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConfigurationManager.View.UserControls
{
    /// <summary>
    /// Interaction logic for TabsView.xaml
    /// </summary>
    public partial class TabsView : UserControl
    {
        public TabsView()
        {
            InitializeComponent();
        }


        private void CloseCurrentTab(object sender, RoutedEventArgs e)
        {
            ProjectExplorerItem current_item = ((ProjectExplorerItem)((sender as Button).Parent as DockPanel).DataContext);
            current_item.CloseTab();
        }
    }
}
