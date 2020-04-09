using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class ConfigurationBool : ConfigurationVariable
    {
        bool _value;
        bool IsExplicit { get; set; }
        public ConfigurationBool(bool value, bool is_explicit)
        {
            _value = value;
            IsExplicit = is_explicit;
        }

        public bool Value
        {
            get
            {
                return _value;
            }
        }

        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationBool(fromJson.ToObject<bool>(), false);
            } else if (IsExplicitType(fromJson))
            {
                return new ConfigurationBool(fromJson["value"].ToObject<bool>(), true);
            }
            return null;
        }

        public static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Boolean;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson)["type"].ToString().Equals("bool");
        }

        public override bool IsValidValue(object o)
        {
            throw new NotImplementedException();
        }

        public override object GetDictionary()
        {
            if (IsExplicit)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["type"] = "bool";
                dict["value"] = Value;
                return dict;
            }
            return Value;
        }

        public override bool IsDirty()
        {
            return Dirty;
        }
    }
}
