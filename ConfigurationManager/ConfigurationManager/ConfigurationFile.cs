using ConfigurationManager.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ConfigurationManager
{
    public class ConfigurationFile
    {
        CompositeConfiguraionVariable _content;

        public ConfigurationFile(JObject array)
        {
            _content = new CompositeConfiguraionVariable(array);
        }
    }
}
