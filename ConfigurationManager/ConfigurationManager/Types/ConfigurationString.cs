using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class StringType
    {
        public string Value { get; set; }

        public StringType(string value)
        {
            Value = value;
        }
    }
    public class ConfigurationString: ConfigurationVariable<StringType>
    {
        bool IsExplicit { get; set; }

        public StringType Value { get; set; }

        public ConfigurationString(string val, bool is_explicit = false)
        {
            Value = new StringType(val);
            IsExplicit = is_explicit;
        }
        
        public static new ConfigurationString TryConvert(JToken fromJson)
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
                dict["value"] = Value.Value;
                return dict;
            }
            return Value.Value;
        }

        public override bool IsDirty()
        {
            return Dirty;
        }

        public override void Update(StringType new_value)
        {
            Value = new_value;
        }
    }
}
