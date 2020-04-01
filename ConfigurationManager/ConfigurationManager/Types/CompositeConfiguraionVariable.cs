using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    class CompositeConfiguraionVariable: IConfigurationVariable
    {
        private Dictionary<String, IConfigurationVariable> _variables;

        public CompositeConfiguraionVariable(JObject array)
        {
            _variables = new Dictionary<String, IConfigurationVariable>();
            foreach (KeyValuePair<String, JToken> value in array)
            {
                _variables[value.Key] = IConfigurationVariable.ConvertJsonToConfiguration(value.Value);
            }
        }

        public static IConfigurationVariable TryConvert(JToken fromJson)
        {
            return new CompositeConfiguraionVariable((JObject)fromJson);
        }

        public static bool IsRelevantType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object;
        }

        public bool IsValidValue(object o)
        {
            throw new NotImplementedException();
        }
    }
}
