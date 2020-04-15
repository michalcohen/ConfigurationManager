using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class BoolType
    {
        public bool Value { get; set; }

        public BoolType(bool value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    public class ConfigurationBool : ConfigurationVariable<BoolType>
    {
        public BoolType Value { get; set; }
        bool IsExplicit { get; set; }
        public ConfigurationBool(bool value, Changable father, bool is_explicit = false, string name="") : base(father, Brushes.Magenta, name, is_explicit)
        {
            Value = new BoolType(value);
        }

        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationBool(fromJson.ToObject<bool>(), father, false, name);
            } else if (IsExplicitType(fromJson))
            {
                return new ConfigurationBool(fromJson["value"].ToObject<bool>(), father, true, name);
            }
            return null;
        }

        public static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Boolean;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson)["type"].ToString().Equals("bool");
        }


        public override object GetDictionary()
        {
            if (IsExplicit)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["type"] = "bool";
                dict["value"] = Value.Value;
                return dict;
            }
            return Value.Value;
        }

        public override void Update(BoolType new_value)
        {
            Value = new_value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }
}
