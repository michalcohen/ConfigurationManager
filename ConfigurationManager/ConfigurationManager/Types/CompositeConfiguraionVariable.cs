using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    class CompositeConfiguraionVariable: ConfigurationVariable
    {
        private Dictionary<String, ConfigurationVariable> _variables;

        public CompositeConfiguraionVariable(JObject array)
        {
            _variables = new Dictionary<String, ConfigurationVariable>();
            foreach (KeyValuePair<String, JToken> value in array)
            {
                _variables[value.Key] = ConfigurationVariable.ConvertJsonToConfiguration(value.Value);
            }
        }

        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            return new CompositeConfiguraionVariable((JObject)fromJson);
        }

        public static new bool IsRelevantType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && !((JObject)fromJson).ContainsKey("type");
        }

        public override bool IsValidValue(object o)
        {
            throw new NotImplementedException();
        }
    }
}
