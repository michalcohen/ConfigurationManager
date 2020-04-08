using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class ConfigurationString: ConfigurationVariable
    {
        string _value;
        bool IsExplicit { get; set; }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        public ConfigurationString(string val, bool is_explicit)
        {
            _value = val;
            IsExplicit = is_explicit;
        }

        public override bool IsValidValue(Object o)
        {
            return true;
        }
        
        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationString(fromJson.ToObject<string>(), false);
            } else if (IsExplicitType(fromJson))
            {
                return new ConfigurationString(fromJson["value"].ToObject<string>(), true);
            }
            return null;
        }

        private static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.String;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson)["type"].ToString().Equals("string");
        }

        public override object GetDictionary()
        {
            if (IsExplicit)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["type"] = "string";
                dict["value"] = Value;
                return dict;
            }
            return Value;
        }
    }
}
