using ConfigurationManager.GUIComponents.EditValuesWindows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class StringType: InnerType<string>
    {
        public StringType(string value, bool is_explicit) : base(value, is_explicit)
        {}
    }
    
    public class ConfigurationString: ConfigurationVariable<string>
    {
        public ConfigurationString(string val, Changable father = null, bool is_explicit = false, string name=""): base(father, Brushes.DarkKhaki, name, is_explicit)
        {
            Value = new StringType(val, is_explicit);
        }

        public override void OpenEditWindow()
        {
            (new StringEdit(this)).Show();
        }
        
        public static new ConfigurationString TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationString(fromJson.ToObject<string>(), father, false, name);
            } else if (IsExplicitType(fromJson))
            {
                return new ConfigurationString(fromJson["value"].ToObject<string>(), father, true, name);
            }
            return null;
        }

        private static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.String;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson)["type"].ToString().Equals("string");
        }

    }
}
