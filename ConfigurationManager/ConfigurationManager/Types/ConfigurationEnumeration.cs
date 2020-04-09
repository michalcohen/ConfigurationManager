using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class EnumType
    {
        public string Value { get; }

        public bool IsGlobalEnum { get; }

        public string EnumName { get; }

        public List<string> EnumValues { get; }

        public EnumType(string value, string enum_name)
        {
            Value = value;
            EnumName = enum_name;
            IsGlobalEnum = true;
        }

        public EnumType(string value, List<string> enum_values)
        {
            Value = value;
            EnumValues = enum_values;
            IsGlobalEnum = false;
        }

        public object GetDictionary()
        {
            if (IsGlobalEnum)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    ["type"] = EnumName,
                    ["value"] = Value
                };
                return dict;
            }
            else
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    ["type"] = EnumValues,
                    ["value"] = Value
                };
                return dict;
            }
        }

    }
    
    public class ConfigurationEnumeration: ConfigurationVariable<EnumType>
    {
        public EnumType Value { get; set; }

        public ConfigurationEnumeration(string value, string enum_name)
        {
            Value = new EnumType(value, enum_name);
        }

        public ConfigurationEnumeration(string value, List<string> enum_values)
        {
            Value = new EnumType(value, enum_values);
        }

        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            if (fromJson.Type != JTokenType.Object)
            {
                return null;
            }
            JObject j = (JObject)fromJson;
            if (!j.ContainsKey("type"))
            {
                return null;
            }
            JToken t = j["type"];
            if (t.Type == JTokenType.String)
            {
                string name = t.ToObject<string>();
                if (Enums.HasEnum(name))
                {
                    return new ConfigurationEnumeration(j["value"].ToObject<string>(), name);
                }
            }
            else if (t.Type == JTokenType.Array)
            {
                JArray vals = (JArray)t;
                if (vals[0].Type != JTokenType.String)
                {
                    return null;
                }
                List<string> values = new List<string>();
                foreach (JToken value in vals)
                {
                    values.Add(value.ToObject<string>());
                }
                return new ConfigurationEnumeration(j["value"].ToObject<string>(), values);
            }
            return null;
        }


        public override object GetDictionary()
        {
            return Value.GetDictionary();
        }

        public override bool IsDirty()
        {
            return Dirty;
        }

        public override void Update(EnumType new_value)
        {
            Value = new_value;
        }
    }
}
