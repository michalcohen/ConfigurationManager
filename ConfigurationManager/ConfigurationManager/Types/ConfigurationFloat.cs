using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class ConfigurationFloat: ConfigurationVariable
    {
        private float _lowest_value, _highest_value;
        private bool _isValid;

        float _value;

        public ConfigurationFloat(float val, float lowest=float.MinValue, float highest=float.MaxValue)
        {
            _value = val;
            _lowest_value = lowest;
            _highest_value = highest;
            if (lowest > highest)
            {
                throw new Exception("Invlid bounderies");
            }
        }

        public float Value
        {
            get
            {
                return _value;
            }
        }

        public float LowestValue
        {
            get
            {
                return _lowest_value;
            }
            set
            {
                if (_lowest_value != value)
                {
                    _lowest_value = value;
                    // OnPropertyChanged();
                    //SetIsValid();
                }
            }
        }

        public float HighestValue
        {
            get
            {
                return _highest_value;
            }
            set
            {
                if (_highest_value != value)
                {
                    _highest_value = value;
                    // OnPropertyChanged();
                    //SetIsValid();
                }
            }
        }

        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationFloat(fromJson.ToObject<float>());
            } else if (IsExplicitType(fromJson))
            {
                JObject j = (JObject)fromJson;
                float l = j.ContainsKey("lower_bound") ? j["lower_bound"].ToObject<float>() : float.MinValue;
                float h = j.ContainsKey("higher_bound") ? j["higher_bound"].ToObject<float>() : float.MaxValue;
                return new ConfigurationFloat(fromJson["value"].ToObject<float>(), lowest: l, highest: h);
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

        public override bool IsValidValue(object o)
        {
            throw new NotImplementedException();
        }
    }
}
