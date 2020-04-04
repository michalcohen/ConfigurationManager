using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class ConfigurationString: ConfigurationVariable
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
        
        public override bool IsValidValue(Object o)
        {
            return true;
        }
        
        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationString(fromJson.ToObject<string>());
            }
            return new ConfigurationString(fromJson["value"].ToObject<string>());
        }

        private static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.String;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson)["type"].ToString().Equals("string");
        }

        public static new bool IsRelevantType(JToken fromJson)
        {
            return IsImplicitType(fromJson) || IsExplicitType(fromJson);
        }
    }
}
