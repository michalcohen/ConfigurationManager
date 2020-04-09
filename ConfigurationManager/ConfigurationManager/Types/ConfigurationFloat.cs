using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class FloatType
    {
        public float Value { get; set; }

        public float LowestValue { get; set; }

        public float HighestValue { get; set; }

        public FloatType(float value, float lowest, float highest)
        {

        }
    }
    public class ConfigurationFloat: ConfigurationVariable<FloatType>
    {
        bool IsExplicit { get; set; }

        public FloatType Value { get; set; }

        public ConfigurationFloat(float val, bool is_explicit = false, float lowest=float.MinValue, float highest=float.MaxValue)
        {
            if (lowest > highest)
            {
                throw new Exception("Invlid bounderies");
            }
            IsExplicit = is_explicit;
            Value = new FloatType(val, lowest, highest);
        }

        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationFloat(fromJson.ToObject<float>(), false);
            } else if (IsExplicitType(fromJson))
            {
                JObject j = (JObject)fromJson;
                float l = j.ContainsKey("lower_bound") ? j["lower_bound"].ToObject<float>() : float.MinValue;
                float h = j.ContainsKey("higher_bound") ? j["higher_bound"].ToObject<float>() : float.MaxValue;
                return new ConfigurationFloat(fromJson["value"].ToObject<float>(), true, lowest: l, highest: h);
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
                dict["value"] = Value;
                return dict;
            }
            return Value;
        }

        public override bool IsDirty()
        {
            return Dirty;
        }

        public override void Update(FloatType new_value)
        {
            Value = new_value;
        }
    }
}
