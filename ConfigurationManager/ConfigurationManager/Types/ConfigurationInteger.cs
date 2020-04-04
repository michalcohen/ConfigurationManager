using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class ConfigurationInteger: ConfigurationVariable
    {
        private int _lowest_value, _highest_value;
        private bool _isValid;
        int _value;

        public ConfigurationInteger(int val, int lowest=int.MinValue, int highest = int.MaxValue)
        {
            _value = val;
            _lowest_value = lowest;
            _highest_value = highest;
        }

        public int Value
        {
            get
            {
                return _value;
            }
        }

        public int LowestValue
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

        public int HighestValue
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

        /// <summary>
        /// Indicates whether the model is in a valid state or not.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    //OnPropertyChanged();
                }
            }
        }

        private void SetIsValid()
        {
            //IsValid = !string.IsNullOrEmpty(Name);
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        //private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public override bool IsValidValue(object o)
        {
            int x = (int)o;
            return x >= LowestValue || x <= HighestValue;
        }

        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationInteger(fromJson.ToObject<int>());
            } else if (IsExplicitType(fromJson))
            {
                JObject j = (JObject)fromJson;
                int l = j.ContainsKey("lower_bound") ? j["lower_bound"].ToObject<int>() : int.MinValue;
                int h = j.ContainsKey("higher_bound") ? j["higher_bound"].ToObject<int>() : int.MaxValue;
                return new ConfigurationInteger(fromJson["value"].ToObject<int>(), lowest: l, highest: h);
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
