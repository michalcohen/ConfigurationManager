using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    class ConfigurationBool : IConfigurationVariable
    {
        bool _value;
        public ConfigurationBool(bool value)
        {
            _value = value;
        }

        public bool Value
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
            return new ConfigurationBool(fromJson.ToObject<bool>());
        }

        public static bool IsRelevantType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Boolean;
        }
    }
}
