using ConfigurationManager.View.UserControls.EditValuesWindows;
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
        public StringType(ConfigurationString father, string value, bool is_explicit) : base(father, value, is_explicit)
        {}

        public StringType(StringType other, ConfigurationVariable father = null) : base(other, father) { }

        public override InnerType<string> Clone(ConfigurationVariable father = null)
        {
            return new StringType(this, father);
        }
    }
    
    public class ConfigurationString: ConfigurationVariable<StringType, string>
    {
        private static Brush brush = Brushes.DarkKhaki;
        public ConfigurationString(string val, Changable father = null, bool is_explicit = false, string name="", string description = "", string notes = "") : base(father, ConfigurationString.brush, name, is_explicit, description, notes)
        {
            Value = new StringType(this, val, is_explicit);
        }

        public ConfigurationString(ConfigurationString other, Changable father = null): base(other, father)
        {}

        public ConfigurationString(): base()
        {
            Value = new StringType(this, "", true);
            FontColor = ConfigurationString.brush;
        }

        public override UserControl GetEditView()
        {
            return new EditString(Value);
        }

        public static new ConfigurationString TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationString(fromJson.ToObject<string>(), father, false, name);
            } else if (IsExplicitType(fromJson))
            {
                JObject x = fromJson as JObject;
                string description = x.ContainsKey("description") ? x["description"].ToString() : "";
                string notes = x.ContainsKey("notes") ? x["notes"].ToString() : "";
                return new ConfigurationString(fromJson["value"].ToObject<string>(), father, true, name, description, notes);
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

        public override ConfigurationVariable Clone(Changable father = null)
        {
            return new ConfigurationString(this, father);
        }
    }
}
