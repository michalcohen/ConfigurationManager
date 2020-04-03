using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    class ConfigurationFloat: ConfigurationVariable
    {
        private float _lowest_value, _highest_value;
        private bool _isValid;

        float _value;

        public ConfigurationFloat(float val, float lowest=float.MinValue, float highest=float.MaxValue)
        {
            _value = val;
            _lowest_value = lowest;
            _highest_value = highest;
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
            }
            return new ConfigurationFloat(fromJson["value"].ToObject<float>());
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

        public static new bool IsRelevantType(JToken fromJson)
        {
            return IsImplicitType(fromJson) || IsExplicitType(fromJson);
        }
    }
}
