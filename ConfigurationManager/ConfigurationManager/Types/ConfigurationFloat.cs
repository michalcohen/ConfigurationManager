using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    class ConfigurationFloat: IConfigurationVariable
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

        public bool IsValidValue(object o)
        {
            float x = (float)o;
            return x >= LowestValue || x <= HighestValue;
        }

        public static IConfigurationVariable TryConvert(JToken fromJson)
        {
            return new ConfigurationFloat(fromJson.ToObject<float>());
        }

        public static bool IsRelevantType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Float;
        }
    }
}
