using ConfigurationManager.View.UserControls.EditValuesWindows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class BoolType: InnerType<bool>
    {
        public BoolType(ConfigurationBool father, bool value, bool is_explicit) : base(father, value, is_explicit)
        {}

        public BoolType(BoolType other, ConfigurationVariable father = null) : base(other, father) { }
        public override InnerType<bool> Clone(ConfigurationVariable father = null)
        {
            return new BoolType(this, father);
        }
    }
    
    public class ConfigurationBool : ConfigurationVariable<BoolType, bool>
    {
        private static Brush brush = Brushes.Magenta;
        public ConfigurationBool(bool value, Changable father = null, bool is_explicit = false, string name = "", string description = "", string notes = "") : base(father, ConfigurationBool.brush, name, is_explicit, description, notes)
        {
            Value = new BoolType(this, value, is_explicit);
        }

        public ConfigurationBool(ConfigurationBool other, Changable father = null): base(other, father) { }

        public ConfigurationBool(): base()
        {
            FontColor = ConfigurationBool.brush;
            Value = new BoolType(this, false, true);
        }

        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationBool(fromJson.ToObject<bool>(), father, false, name);
            } else if (IsExplicitType(fromJson))
            {
                JObject x = fromJson as JObject;
                string description = x.ContainsKey("description") ? x["description"].ToString() : "";
                string notes = x.ContainsKey("notes") ? x["notes"].ToString() : "";
                return new ConfigurationBool(fromJson["value"].ToObject<bool>(), father: father, name: name, is_explicit: true, description: description, notes: notes);
            }
            return null;
        }

        public static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Boolean;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson).ContainsKey("type") && ((JObject)fromJson)["type"].ToString().Equals("boolean");
        }

        public override ConfigurationVariable Clone(Changable father = null)
        {
            return new ConfigurationBool(this, father);
        }

        public override UserControl GetEditView()
        {
            return new EditBool(Value);
        }
    }
}
