using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class EnumType: InnerType<string>
    {
        public bool IsGlobalEnum { get; }

        public string EnumName { get; }

        public List<string> EnumValues { get; }

        public EnumType(string value, string enum_name): base(value, true)
        {
            EnumName = enum_name;
            IsGlobalEnum = true;
        }

        public EnumType(string value, List<string> enum_values): base(value, true)
        {
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

        public override string ToString()
        {
            return Value.ToString();
        }

    }
    
    public class ConfigurationEnumeration: ConfigurationVariable<string>
    {
        public ConfigurationEnumeration(string value, Changable father = null, string enum_name = "", string name="") : base(father, Brushes.DarkGoldenrod, name, true)
        {
            Value = new EnumType(value, enum_name);
        }

        public ConfigurationEnumeration(string value, Changable father = null, List<string> enum_values = null, string name="") : base(father, Brushes.DarkKhaki, name, true)
        {
            Value = new EnumType(value, enum_values);
        }

        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
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
                string type_name = t.ToObject<string>();
                if (Enums.HasEnum(type_name))
                {
                    return new ConfigurationEnumeration(j["value"].ToObject<string>(), father, type_name, name);
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
                return new ConfigurationEnumeration(j["value"].ToObject<string>(), father, values, name);
            }
            return null;
        }

    }
}
