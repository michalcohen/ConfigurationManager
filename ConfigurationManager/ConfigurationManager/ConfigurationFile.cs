using ConfigurationManager.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ConfigurationManager
{
    public class ConfigurationFile
    {
        public CompositeConfiguraionVariable Content { get; }

        public ConfigurationFile(JObject array)
        {
            Content= new CompositeConfiguraionVariable(array);
        }

        public Dictionary<string, object> GetDictionary()
        {
            return Content.GetDictionary() as Dictionary<string, object>;
        }
    }
}
