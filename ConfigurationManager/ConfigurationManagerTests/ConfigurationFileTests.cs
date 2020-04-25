using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConfigurationManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ConfigurationManager.Model.Types;
using ConfigurationManager.Model;

namespace ConfigurationManager.Tests
{
    [TestClass()]
    public class ConfigurationFileTests
    {
        [TestMethod()]
        public void ConfigurationFileTest()
        {

        }

        [TestMethod()]
        public void ExampleTest()
        {
            GlobalEnums.LoadEnums("Resources\\ConfigurationsForExample");
            using StreamReader r = new StreamReader("Resources\\ConfigurationsForExample\\Example.json");
            string json = r.ReadToEnd();
            JObject array = (JObject)JsonConvert.DeserializeObject(json);
            ConfigurationFile c = new ConfigurationFile(array);
            Assert.IsInstanceOfType(c.Content["use_bloop"], typeof(ConfigurationComposite));
            Assert.IsInstanceOfType(c.Content["folder"], typeof(ConfigurationString));
            Assert.IsInstanceOfType(c.Content["index"], typeof(ConfigurationInteger));
            Assert.IsInstanceOfType(c.Content["threshold"], typeof(ConfigurationFloat));
            Assert.IsInstanceOfType(c.Content["choose"], typeof(ConfigurationList));
            Assert.IsInstanceOfType(c.Content["bloop"], typeof(ConfigurationList));
            Assert.IsInstanceOfType(c.Content["run_with_profiler"], typeof(ConfigurationBool));
            Assert.IsInstanceOfType(c.Content["method"], typeof(ConfigurationString));
            Assert.IsInstanceOfType(c.Content["try_bool"], typeof(ConfigurationBool));
            Assert.IsInstanceOfType(c.Content["try_int"], typeof(ConfigurationInteger));
            Assert.IsInstanceOfType(c.Content["try_int_lower_bound"], typeof(ConfigurationInteger));
            Assert.IsInstanceOfType(c.Content["try_int_higher_bound"], typeof(ConfigurationInteger));
            Assert.IsInstanceOfType(c.Content["try_int_both_bounds"], typeof(ConfigurationInteger));
            Assert.IsInstanceOfType(c.Content["try_float"], typeof(ConfigurationFloat));
            Assert.IsInstanceOfType(c.Content["try_float_lower_bound"], typeof(ConfigurationFloat));
            Assert.IsInstanceOfType(c.Content["try_float_higher_bound"], typeof(ConfigurationFloat));
            Assert.IsInstanceOfType(c.Content["try_float_both_bounds"], typeof(ConfigurationFloat));
            Assert.IsInstanceOfType(c.Content["try_string"], typeof(ConfigurationString));
            Assert.IsInstanceOfType(c.Content["try_enum_inplace"], typeof(ConfigurationEnumeration));
            Assert.IsInstanceOfType(c.Content["try_enum_global"], typeof(ConfigurationEnumeration));


            Assert.AreEqual(((ConfigurationBool)((ConfigurationComposite)c.Content["use_bloop"])["first"]).Value.Value, true);
            Assert.AreEqual(((ConfigurationBool)((ConfigurationComposite)c.Content["use_bloop"])["second"]).Value.Value, true);
            Assert.AreEqual(((ConfigurationBool)((ConfigurationComposite)c.Content["use_bloop"])["third"]).Value.Value, false);

            Assert.AreEqual(((ConfigurationString)c.Content["folder"]).Value.Value, "C:\\Users\\Trumpy\\Documents");
            Assert.AreEqual(((ConfigurationInteger)c.Content["index"]).Value.Value, 3);
            Assert.AreEqual(((ConfigurationFloat)c.Content["threshold"]).Value.Value, 5.8, 0.00001);
            Assert.AreEqual(((ConfigurationInteger)((ConfigurationList)c.Content["choose"]).Variables[2]).Value.Value, 3);
            Assert.AreEqual(((ConfigurationBool)c.Content["run_with_profiler"]).Value.Value, false);
            Assert.AreEqual(((ConfigurationString)c.Content["method"]).Value.Value, "soft");
            Assert.AreEqual(((ConfigurationBool)c.Content["try_bool"]).Value.Value, true);
            Assert.AreEqual(((ConfigurationInteger)c.Content["try_int"]).Value.Value, 5);
            Assert.AreEqual(((ConfigurationFloat)c.Content["try_float"]).Value.Value, 5.7, 0.00001);
            Assert.AreEqual((((ConfigurationFloat)c.Content["try_float_both_bounds"]).Value as FloatType).LowestValue, 4.8, 0.00001);
            Assert.AreEqual((((ConfigurationFloat)c.Content["try_float_both_bounds"]).Value as FloatType).HighestValue, 7.0, 0.00001);
            Assert.AreEqual(((ConfigurationString)c.Content["try_string"]).Value.Value, "bananana");
            
            Assert.AreEqual(((ConfigurationEnumeration)c.Content["try_enum_inplace"]).Value.Value, "dummy_method");
            Assert.AreEqual((((ConfigurationEnumeration)c.Content["try_enum_inplace"]).Value as EnumType).IsGlobalEnum, false);
            Assert.AreEqual((((ConfigurationEnumeration)c.Content["try_enum_inplace"]).Value as EnumType).EnumValues.Count, 3);
            Assert.AreEqual((((ConfigurationEnumeration)c.Content["try_enum_inplace"]).Value as EnumType).EnumValues[0], "dummy_method");
            Assert.AreEqual((((ConfigurationEnumeration)c.Content["try_enum_inplace"]).Value as EnumType).EnumValues[1], "parallel_method");
            Assert.AreEqual((((ConfigurationEnumeration)c.Content["try_enum_inplace"]).Value as EnumType).EnumValues[2], "serial_method");


            Assert.AreEqual(((ConfigurationEnumeration)c.Content["try_enum_global"]).Value.Value, "Blue");
            Assert.AreEqual((((ConfigurationEnumeration)c.Content["try_enum_global"]).Value as EnumType).IsGlobalEnum, true);
            Assert.AreEqual((((ConfigurationEnumeration)c.Content["try_enum_global"]).Value as EnumType).EnumName, "Colors");
            Assert.IsTrue(GlobalEnums.GetGlobalEnum("Colors").ContainsValue("Blue"));
        }
    }
}