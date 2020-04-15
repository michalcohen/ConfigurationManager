using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class IntegerType
    {
        public int LowestValue { get; set; }
        public int HighestValue { get; set; }
        public int Value { get; set; }

        public IntegerType(int value, int lowest, int highest)
        {
            Value = value;
            LowestValue = lowest;
            HighestValue = highest;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    public class ConfigurationInteger: ConfigurationVariable<IntegerType>
    {
        public IntegerType Value { get; set; }
        
        bool IsExplicit { get; set; }

        public ConfigurationInteger(int val, bool is_explicit = false, int lowest=int.MinValue, int highest = int.MaxValue, string name="")
        {
            FontColor = Brushes.Green;
            ConfigurationName = name;
            IsExplicit = is_explicit;
            Value = new IntegerType(val, lowest, highest);
        }

        public static new ConfigurationInteger TryConvert(string name, JToken fromJson)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationInteger(fromJson.ToObject<int>(), false, name: name);
            } else if (IsExplicitType(fromJson))
            {
                JObject j = (JObject)fromJson;
                int l = j.ContainsKey("lower_bound") ? j["lower_bound"].ToObject<int>() : int.MinValue;
                int h = j.ContainsKey("higher_bound") ? j["higher_bound"].ToObject<int>() : int.MaxValue;
                return new ConfigurationInteger(fromJson["value"].ToObject<int>(), true, lowest: l, highest: h, name:name);
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

        public override object GetDictionary()
        {
            if (IsExplicit)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["type"] = "int";
                dict["value"] = Value.Value;
                if (Value.HighestValue < int.MaxValue)
                {
                    dict["higher_bound"] = Value.HighestValue;
                }
                if (Value.LowestValue > int.MinValue)
                {
                    dict["lower_bound"] = Value.LowestValue;
                }
                return dict;
            }
            return Value.Value;
        }

        public override bool IsDirty()
        {
            return Dirty;
        }

        public override void Update(IntegerType new_value)
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
