using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    class ConfigurationList : ConfigurationVariable
    {
        List<ConfigurationVariable> _variables;

        public ConfigurationList(JArray array)
        {
            _variables = new List<ConfigurationVariable>();
            foreach (JToken value in array)
            {
                _variables.Add(ConfigurationVariable.ConvertJsonToConfiguration(value));
            }
        }

        public static ConfigurationVariable TryConvert(JToken fromJson)
        {
            return new ConfigurationList((JArray)fromJson);
        }

        public static bool IsRelevantType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Array;
        }

        public override bool IsValidValue(object o)
        {
            throw new NotImplementedException();
        }
    }
}
