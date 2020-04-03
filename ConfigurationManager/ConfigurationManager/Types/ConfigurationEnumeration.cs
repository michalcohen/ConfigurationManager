using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    class ConfigurationEnumeration: ConfigurationVariable
    {
        private bool _isValid;

        ConfigurationEnumerationType _type;
        string _value;

        public string Value
        {
            get
            {
                return _value;
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

        public static ConfigurationVariable TryConvert(JToken fromJson)
        {
            // Not correct
            return new ConfigurationFloat(fromJson.ToObject<float>());
        }

        public static bool IsRelevantType(JToken fromJson)
        {
            return false;
        }

        public override bool IsValidValue(object o)
        {
            throw new NotImplementedException();
        }
    }
}
