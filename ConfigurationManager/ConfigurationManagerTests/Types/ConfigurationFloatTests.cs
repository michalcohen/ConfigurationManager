using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConfigurationManager.Model.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ConfigurationManager.Model.Types.Tests
{
    [TestClass()]
    public class ConfigurationFloatTests
    {
        [TestMethod()]
        public void ConfigurationFloatTest()
        {
            ConfigurationFloat f = new ConfigurationFloat((float)5.0);
            ConfigurationFloat fl = new ConfigurationFloat((float)5.0, lowest: (float)4.0);
            ConfigurationFloat fh = new ConfigurationFloat((float)5.0, highest: (float)6.0);
            ConfigurationFloat flh = new ConfigurationFloat((float)5.0, highest: (float)6.0, lowest: (float)3.4);

            Assert.AreEqual(5.0, f.Value.Value);
            Assert.AreEqual(float.MaxValue, (f.Value as FloatType).HighestValue);
            Assert.AreEqual(float.MinValue, (f.Value as FloatType).LowestValue);
            
            Assert.AreEqual(5.0, fl.Value.Value);
            Assert.AreEqual(float.MaxValue, (fl.Value as FloatType).HighestValue);
            Assert.AreEqual(4.0, (fl.Value as FloatType).LowestValue);

            Assert.AreEqual(5.0, fh.Value.Value);
            Assert.AreEqual(6.0, (fh.Value as FloatType).HighestValue);
            Assert.AreEqual(float.MinValue, (fh.Value as FloatType).LowestValue);

            Assert.AreEqual(5.0, flh.Value.Value);
            Assert.AreEqual(6.0, (flh.Value as FloatType).HighestValue);
            Assert.AreEqual(3.4, (flh.Value as FloatType).LowestValue, 0.0001);

            Assert.ThrowsException<Exception>(() => new ConfigurationFloat((float)5.0, highest: (float)2.0, lowest: (float)3.4));
        }

        

        [TestMethod()]
        public void IsExplicitTypeTest()
        {

        }

        [TestMethod()]
        public void IsValidValueTest()
        {

        }

        [TestMethod()]
        public void IsRelevantTypeTest()
        {

        }
    }
}