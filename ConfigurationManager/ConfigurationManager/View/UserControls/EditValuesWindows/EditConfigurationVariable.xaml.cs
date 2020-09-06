using ConfigurationManager.Model.Types;
using ConfigurationManager.View.UserControls.EditValuesWindows;
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
using System.Windows.Shapes;

namespace ConfigurationManager.View.UserControls.EditValuesWindows
{
    /// <summary>
    /// Interaction logic for EditConfigurationVariable.xaml
    /// </summary>
    public partial class EditConfigurationVariable : UserControl
    {
        public ConfigurationVariable original_CV;
        public EditConfigurationVariable()
        {
            InitializeComponent();
        }

        private bool UserAwerOfMetaDataWithImplicit()
        {
            string messageBoxText = "Defining variable to be implictly represented might cause information lost.\nAre you sure you want to save these changes?";
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            return result == MessageBoxResult.Yes;
        }
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            ConfigurationVariable cv = DataContext as ConfigurationVariable;
            if (original_CV == null)
            {
                if (!(cv.IsImplicit && cv.ContainsMetaData()) || UserAwerOfMetaDataWithImplicit())
                {
                    original_CV = cv;
                } else
                {
                    return;
                }
                    
            } else
            {
                if (!(cv.IsImplicit && original_CV.IsExplicit && cv.ContainsMetaData()) || UserAwerOfMetaDataWithImplicit())
                {
                    original_CV.UpdateBy(cv);
                } else
                {
                    return;
                }
            }
            Window.GetWindow(this).Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
    }
}
