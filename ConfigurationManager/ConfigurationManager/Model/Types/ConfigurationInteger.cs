using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class IntegerType: BoundedInnerType<int>
    {
        public IntegerType(int value, int lowest, int highest, bool is_explicit) : base(value, lowest, highest, is_explicit)
        {}
    }
    public class ConfigurationInteger: ConfigurationVariable<int>
    {
        public ConfigurationInteger(int val, Changable father = null, bool is_explicit = false, int lowest=int.MinValue, int highest = int.MaxValue, string name="") : base(father, Brushes.Green, name, is_explicit)
        {
            Value = new IntegerType(val, lowest, highest, is_explicit);
        }

        public static new ConfigurationInteger TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationInteger(fromJson.ToObject<int>(), father, false, name: name);
            } else if (IsExplicitType(fromJson))
            {
                JObject j = (JObject)fromJson;
                int l = j.ContainsKey("lower_bound") ? j["lower_bound"].ToObject<int>() : int.MinValue;
                int h = j.ContainsKey("higher_bound") ? j["higher_bound"].ToObject<int>() : int.MaxValue;
                return new ConfigurationInteger(fromJson["value"].ToObject<int>(), father, true, lowest: l, highest: h, name:name);
            }
            return null;
        }

        private static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Integer;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson)["type"].ToString().Equals("int");
        }

    }
}
