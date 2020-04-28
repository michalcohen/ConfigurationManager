using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConfigurationManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;
using System.Windows;

namespace ConfigurationManager.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        [TestMethod()]
        public void MainWindowTest()
        {
                MainWindow.path = "Resources\\ConfigurationsForExample";
                Window main_window = new MainWindow();
        }
    }
}