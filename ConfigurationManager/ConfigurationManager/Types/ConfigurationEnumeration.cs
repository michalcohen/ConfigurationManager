using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class ConfigurationEnumeration: ConfigurationVariable
    {
        private bool _isValid;

        public string Value { get; }

        public bool IsGlobalEnum { get; }

        public string EnumName { get; }

        public List<string> EnumValues { get; }

        public ConfigurationEnumeration(string value, string enum_name)
        {
            Value = value;
            EnumName = enum_name;
            IsGlobalEnum = true;
        }

        public ConfigurationEnumeration(string value, List<string> enum_values)
        {
            Value = value;
            EnumValues = enum_values;
            IsGlobalEnum = false;
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

        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            if (fromJson.Type != JTokenType.Object)
            {
                return null;
            }
            JObject j = (JObject)fromJson;
            if (!j.ContainsKey("type"))
            {
                return null;
            }
            JToken t = j["type"];
            if (t.Type == JTokenType.String)
            {
                string name = t.ToObject<string>();
                if (Enums.HasEnum(name))
                {
                    return new ConfigurationEnumeration(j["value"].ToObject<string>(), name);
                }
            }
            else if (t.Type == JTokenType.Array)
            {
                JArray vals = (JArray)t;
                if (vals[0].Type != JTokenType.String)
                {
                    return null;
                }
                List<string> values = new List<string>();
                foreach (JToken value in vals)
                {
                    values.Add(value.ToObject<string>());
                }
                return new ConfigurationEnumeration(j["value"].ToObject<string>(), values);
            }
            return null;
        }

        public override bool IsValidValue(object o)
        {
            throw new NotImplementedException();
        }

        public override object GetDictionary()
        {
            if (IsGlobalEnum)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["type"] = EnumName;
                dict["value"] = Value;
                return dict;
            } else
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["type"] = EnumValues;
                dict["value"] = Value;
                return dict;
            }
        }
    }
}
