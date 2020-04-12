using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace ConfigurationManager.Types
{
    public class BoolType
    {
        public bool Value { get; set; }

        public BoolType(bool value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    public class ConfigurationBool : ConfigurationVariable<BoolType>
    {
        public BoolType Value { get; set; }
        bool IsExplicit { get; set; }
        public ConfigurationBool(bool value, bool is_explicit = false)
        {
            Value = new BoolType(value);
            IsExplicit = is_explicit;
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


        public override object GetDictionary()
        {
            if (IsExplicit)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["type"] = "bool";
                dict["value"] = Value.Value;
                return dict;
            }
            return Value.Value;
        }

        public override bool IsDirty()
        {
            return Dirty;
        }

        public override void Update(BoolType new_value)
        {
            Value = new_value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override Brush GetFontColor()
        {
            return Brushes.OrangeRed;
        }
    }
}
