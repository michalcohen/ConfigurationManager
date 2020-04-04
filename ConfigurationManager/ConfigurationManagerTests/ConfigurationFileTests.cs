using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConfigurationManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ConfigurationManager.Types;

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
            Enums.LoadEnums();
            using StreamReader r = new StreamReader("Configurations\\Example.json");
            string json = r.ReadToEnd();
            JObject array = (JObject)JsonConvert.DeserializeObject(json);
            ConfigurationFile c = new ConfigurationFile(array);
            Assert.IsInstanceOfType(c.Content.Variables["use_bloop"], typeof(CompositeConfiguraionVariable));
            Assert.IsInstanceOfType(c.Content.Variables["folder"], typeof(ConfigurationString));
            Assert.IsInstanceOfType(c.Content.Variables["index"], typeof(ConfigurationInteger));
            Assert.IsInstanceOfType(c.Content.Variables["threshold"], typeof(ConfigurationFloat));
            Assert.IsInstanceOfType(c.Content.Variables["choose"], typeof(ConfigurationList));
            Assert.IsInstanceOfType(c.Content.Variables["bloop"], typeof(ConfigurationList));
            Assert.IsInstanceOfType(c.Content.Variables["run_with_profiler"], typeof(ConfigurationBool));
            Assert.IsInstanceOfType(c.Content.Variables["method"], typeof(ConfigurationString));
            Assert.IsInstanceOfType(c.Content.Variables["try_bool"], typeof(ConfigurationBool));
            Assert.IsInstanceOfType(c.Content.Variables["try_int"], typeof(ConfigurationInteger));
            Assert.IsInstanceOfType(c.Content.Variables["try_int_lower_bound"], typeof(ConfigurationInteger));
            Assert.IsInstanceOfType(c.Content.Variables["try_int_higher_bound"], typeof(ConfigurationInteger));
            Assert.IsInstanceOfType(c.Content.Variables["try_int_both_bounds"], typeof(ConfigurationInteger));
            Assert.IsInstanceOfType(c.Content.Variables["try_float"], typeof(ConfigurationFloat));
            Assert.IsInstanceOfType(c.Content.Variables["try_float_lower_bound"], typeof(ConfigurationFloat));
            Assert.IsInstanceOfType(c.Content.Variables["try_float_higher_bound"], typeof(ConfigurationFloat));
            Assert.IsInstanceOfType(c.Content.Variables["try_float_both_bounds"], typeof(ConfigurationFloat));
            Assert.IsInstanceOfType(c.Content.Variables["try_string"], typeof(ConfigurationString));
            Assert.IsInstanceOfType(c.Content.Variables["try_enum_inplace"], typeof(ConfigurationEnumeration));
            Assert.IsInstanceOfType(c.Content.Variables["try_enum_global"], typeof(ConfigurationEnumeration));


            Assert.AreEqual(((ConfigurationBool)((CompositeConfiguraionVariable)c.Content.Variables["use_bloop"]).Variables["first"]).Value, true);
            Assert.AreEqual(((ConfigurationBool)((CompositeConfiguraionVariable)c.Content.Variables["use_bloop"]).Variables["second"]).Value, true);
            Assert.AreEqual(((ConfigurationBool)((CompositeConfiguraionVariable)c.Content.Variables["use_bloop"]).Variables["third"]).Value, false);

            Assert.AreEqual(((ConfigurationString)c.Content.Variables["folder"]).Value, "C:\\Users\\Trumpy\\Documents");
            Assert.AreEqual(((ConfigurationInteger)c.Content.Variables["index"]).Value, 3);
            Assert.AreEqual(((ConfigurationFloat)c.Content.Variables["threshold"]).Value, 5.8, 0.00001);
            Assert.AreEqual(((ConfigurationInteger)((ConfigurationList)c.Content.Variables["choose"]).Variables[2]).Value, 3);
            Assert.AreEqual(((ConfigurationBool)c.Content.Variables["run_with_profiler"]).Value, false);
            Assert.AreEqual(((ConfigurationString)c.Content.Variables["method"]).Value, "soft");
            Assert.AreEqual(((ConfigurationBool)c.Content.Variables["try_bool"]).Value, true);
            Assert.AreEqual(((ConfigurationInteger)c.Content.Variables["try_int"]).Value, 5);
            Assert.AreEqual(((ConfigurationFloat)c.Content.Variables["try_float"]).Value, 5.7, 0.00001);
            Assert.AreEqual(((ConfigurationFloat)c.Content.Variables["try_float_both_bounds"]).LowestValue, 4.8, 0.00001);
            Assert.AreEqual(((ConfigurationFloat)c.Content.Variables["try_float_both_bounds"]).HighestValue, 7.0, 0.00001);
            Assert.AreEqual(((ConfigurationString)c.Content.Variables["try_string"]).Value, "bananana");
            
            Assert.AreEqual(((ConfigurationEnumeration)c.Content.Variables["try_enum_inplace"]).Value, "dummy_method");
            Assert.AreEqual(((ConfigurationEnumeration)c.Content.Variables["try_enum_inplace"]).IsGlobalEnum, false);
            Assert.AreEqual(((ConfigurationEnumeration)c.Content.Variables["try_enum_inplace"]).EnumValues.Count, 3);
            Assert.AreEqual(((ConfigurationEnumeration)c.Content.Variables["try_enum_inplace"]).EnumValues[0], "dummy_method");
            Assert.AreEqual(((ConfigurationEnumeration)c.Content.Variables["try_enum_inplace"]).EnumValues[1], "parallel_method");
            Assert.AreEqual(((ConfigurationEnumeration)c.Content.Variables["try_enum_inplace"]).EnumValues[2], "serial_method");


            Assert.AreEqual(((ConfigurationEnumeration)c.Content.Variables["try_enum_global"]).Value, "Blue");
            Assert.AreEqual(((ConfigurationEnumeration)c.Content.Variables["try_enum_global"]).IsGlobalEnum, true);
            Assert.AreEqual(((ConfigurationEnumeration)c.Content.Variables["try_enum_global"]).EnumName, "Colors");
            Assert.IsTrue(Enums.EnumsOptions["Colors"].Contains("Blue"));
        }
    }
}