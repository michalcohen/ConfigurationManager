using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    class ConfigurationString: IConfigurationVariable
    {
        string _value;

        public ConfigurationString(string val)
        {
            _value = val;
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }
        public bool IsValidValue(Object o)
        {
            return true;
        }

        public static IConfigurationVariable TryConvert(JToken fromJson)
        {
            return new ConfigurationString(fromJson.ToObject<string>());
        }

        public static bool IsRelevantType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.String;
        }
    }
}
