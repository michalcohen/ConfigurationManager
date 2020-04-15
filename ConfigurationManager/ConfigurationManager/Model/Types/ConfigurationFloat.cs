﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class FloatType
    {
        public float Value { get; set; }

        public float LowestValue { get; set; }

        public float HighestValue { get; set; }

        public FloatType(float value, float lowest, float highest)
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
    public class ConfigurationFloat: ConfigurationVariable<FloatType>
    {
        bool IsExplicit { get; set; }

        public FloatType Value { get; set; }

        public ConfigurationFloat(float val, Changable father, bool is_explicit = false, float lowest=float.MinValue, float highest=float.MaxValue, string name="") : base(father, Brushes.BlueViolet, name, is_explicit)
        {
            if (lowest > highest)
            {
                throw new Exception("Invlid bounderies");
            }
            Value = new FloatType(val, lowest, highest);
        }

        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationFloat(fromJson.ToObject<float>(), father, false, name: name);
            } else if (IsExplicitType(fromJson))
            {
                JObject j = (JObject)fromJson;
                float l = j.ContainsKey("lower_bound") ? j["lower_bound"].ToObject<float>() : float.MinValue;
                float h = j.ContainsKey("higher_bound") ? j["higher_bound"].ToObject<float>() : float.MaxValue;
                return new ConfigurationFloat(fromJson["value"].ToObject<float>(), father, true, lowest: l, highest: h, name:name);
            }
            return null;
        }

        private static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Float;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson)["type"].ToString().Equals("float");
        }

        public override object GetDictionary()
        {
            if (IsExplicit)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["type"] = "float";
                dict["value"] = Value.Value;
                if (Value.HighestValue < float.MaxValue)
                {
                    dict["higher_bound"] = Value.HighestValue;
                }
                if (Value.LowestValue > float.MinValue)
                {
                    dict["lower_bound"] = Value.LowestValue;
                }
                return dict;
            }
            return Value.Value;
        }

        public override void Update(FloatType new_value)
        {
            Value = new_value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }
}
