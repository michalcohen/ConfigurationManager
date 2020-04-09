using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
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
    }
    public class ConfigurationInteger: ConfigurationVariable<IntegerType>
    {
        public IntegerType Value { get; set; }
        
        bool IsExplicit { get; set; }

        public ConfigurationInteger(int val, bool is_explicit = false, int lowest=int.MinValue, int highest = int.MaxValue)
        {
            IsExplicit = is_explicit;
            Value = new IntegerType(val, lowest, highest);
        }

        public static new ConfigurationInteger TryConvert(JToken fromJson)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationInteger(fromJson.ToObject<int>(), false);
            } else if (IsExplicitType(fromJson))
            {
                JObject j = (JObject)fromJson;
                int l = j.ContainsKey("lower_bound") ? j["lower_bound"].ToObject<int>() : int.MinValue;
                int h = j.ContainsKey("higher_bound") ? j["higher_bound"].ToObject<int>() : int.MaxValue;
                return new ConfigurationInteger(fromJson["value"].ToObject<int>(), true, lowest: l, highest: h);
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
                dict["value"] = Value;
                return dict;
            }
            return Value;
        }

        public override bool IsDirty()
        {
            return Dirty;
        }

        public override void Update(IntegerType new_value)
        {
            Value = new_value;
        }
    }
}
