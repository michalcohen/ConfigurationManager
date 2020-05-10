using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConfigurationManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Automation;
using System.Windows;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System.Windows.Controls;
using OpenQA.Selenium.Interactions;

namespace ConfigurationManager.Tests
{
    [TestClass()]
    [Ignore()]
    public class MainWindowTests
    {

        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string WpfAppId = @"D:\ConfigurationManager\ConfigurationManager\ConfigurationManager\ConfigurationManager\bin\Debug\netcoreapp3.1\ConfigurationManager.exe";

        protected static WindowsDriver<WindowsElement> session;
        protected static TreeView main_tree_view;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            if (session == null)
            {
                var appiumOptions = new AppiumOptions();
                appiumOptions.AddAdditionalCapability("app", WpfAppId);
                appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");
                appiumOptions.AddAdditionalCapability("appArguments", "-t");
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            }
        }

        [TestMethod()]
        public void MainWindowTest()
        {
            var project_explorer = session.FindElementByAccessibilityId("ProjectExplorerControl").FindElementByAccessibilityId("configuration_folder_view");
            var tree_view_items = project_explorer.FindElementsByAccessibilityId("file_name");
            Assert.AreEqual(tree_view_items.Count, 3);
            foreach (var i in tree_view_items)
            {
                Actions action = new Actions(session);
                action.DoubleClick(i);
                action.Perform();
                break;
            }
            var TabsControl = session.FindElementByAccessibilityId("TabsControl");
            var tab_items = TabsControl.FindElementsByAccessibilityId("TabContent");
            Assert.AreEqual(tree_view_items.Count, 1);
            foreach (var i in tree_view_items)
            {
                var x = i.FindElementByAccessibilityId("configuration_folder_view");
                break;
            }
            //menu_control.SendKeys("Matteo");
            //session.FindElementByAccessibilityId("sayHelloButton").Click();
            //var txtResult = session.FindElementByAccessibilityId("txtResult");
            //Assert.AreEqual(txtResult.Text, $"Hello {menu_control.Text}");
        }



        [ClassCleanup]
        public static void Cleanup()
        {
            if (session != null)
            {
                session.Close();
                session.Quit();
            }
        }
    }
}