using ConfigurationManager.View.UserControls.EditValuesWindows;
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
        public IntegerType(ConfigurationInteger father, int value, int lowest, int highest, bool is_explicit) : base(father, value, lowest, highest, is_explicit)
        {}

        public IntegerType(IntegerType other): base(other) { }

        public override InnerType<int> Clone()
        {
            return new IntegerType(this);
        }
    }
    public class ConfigurationInteger: ConfigurationVariable<IntegerType, int>
    {
        public ConfigurationInteger(int val, Changable father = null, bool is_explicit = false, int lowest=int.MinValue, int highest = int.MaxValue, string name="") : base(father, Brushes.Green, name, is_explicit)
        {
            Value = new IntegerType(this, val, lowest, highest, is_explicit);
        }

        public ConfigurationInteger(ConfigurationInteger other): base(other)
        {}

        public override UserControl GetEditView()
        {
            return new EditInteger(Value);
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

        public override ConfigurationVariable Clone()
        {
            return new ConfigurationInteger(this);
        }
    }
}
