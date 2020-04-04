using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class ConfigurationBool : ConfigurationVariable
    {
        bool _value;
        public ConfigurationBool(bool value)
        {
            _value = value;
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
                return new ConfigurationBool(fromJson.ToObject<bool>());
            }
            return new ConfigurationBool(fromJson["value"].ToObject<bool>());
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

        public static  new bool IsRelevantType(JToken fromJson)
        {
            return IsImplicitType(fromJson) || IsExplicitType(fromJson);
        }
    }
}
