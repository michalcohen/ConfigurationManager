using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
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

        public override string ToString()
        {
            return Value.ToString();
        }

    }
    
    public class ConfigurationEnumeration: ConfigurationVariable<EnumType>
    {
        public EnumType Value { get; set; }

        public ConfigurationEnumeration(string value, Changable father, string enum_name, string name="")
        {
            Father = father;
            FontColor = Brushes.DarkGoldenrod;
            ConfigurationName = name;
            Value = new EnumType(value, enum_name);
            Variables = new List<ConfigurationVariable>();
        }

        public ConfigurationEnumeration(string value, Changable father, List<string> enum_values, string name="")
        {
            Father = father;
            ConfigurationName = name;
            Value = new EnumType(value, enum_values);
            Variables = new List<ConfigurationVariable>();
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


        public override object GetDictionary()
        {
            return Value.GetDictionary();
        }

        public override void Update(EnumType new_value)
        {
            Value = new_value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override Window GetGUIElementsForEdit()
        {
            return new Window();
        }
    }
}
