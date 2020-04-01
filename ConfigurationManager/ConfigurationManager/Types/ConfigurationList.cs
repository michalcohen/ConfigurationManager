using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    class ConfigurationList : IConfigurationVariable
    {
        List<IConfigurationVariable> _variables;

        public ConfigurationList(JArray array)
        {
            _variables = new List<IConfigurationVariable>();
            foreach (JToken value in array)
            {
                _variables.Add(IConfigurationVariable.ConvertJsonToConfiguration(value));
            }
        }

        public static IConfigurationVariable TryConvert(JToken fromJson)
        {
            return new ConfigurationList((JArray)fromJson);
        }

        public static bool IsRelevantType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Array;
        }

        public bool IsValidValue(object o)
        {
            throw new NotImplementedException();
        }

        
    }
}
