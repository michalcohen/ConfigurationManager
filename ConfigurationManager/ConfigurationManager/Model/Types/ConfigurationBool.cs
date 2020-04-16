using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class BoolType: InnerType<bool>
    {
        public BoolType(bool value, bool is_explicit) : base(value, is_explicit)
        {}
    }
    public class ConfigurationBool : ConfigurationVariable<bool>
    {
        public ConfigurationBool(bool value, Changable father = null, bool is_explicit = false, string name="") : base(father, Brushes.Magenta, name, is_explicit)
        {
            Value = new BoolType(value, is_explicit);
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
    }
}
