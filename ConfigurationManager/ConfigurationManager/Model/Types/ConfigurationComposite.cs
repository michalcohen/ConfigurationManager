using ConfigurationManager.View.UserControls;
using ConfigurationManager.View.UserControls.EditValuesWindows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class ConfigurationComposite: ConfigurationVariable
    {

        private static Brush brush = Brushes.LightGray;
        public ConfigurationComposite(JObject array, Changable father = null, string name="", bool is_explicit = false, string description = "", string notes = "") : base(father, ConfigurationComposite.brush, name, is_explicit, description, notes)
        {
            foreach (KeyValuePair<String, JToken> value in array)
            {
                Variables.Add(ConfigurationVariable.ConvertJsonToConfiguration(value.Key, value.Value, this));
            }
            IsComposite = true;
        }

        public ConfigurationComposite(ConfigurationComposite other, Changable father = null): base(other, father)
        {}

        public ConfigurationComposite(): base() 
        {
            FontColor = ConfigurationComposite.brush;
        }

        public override bool CheckValidity()
        {
            return Variables.All(x => x.IsValid);
        }
        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationComposite(array: (JObject)fromJson, father: father, name: name);
            } else if (IsExplicitType(fromJson))
            {
                JObject x = fromJson as JObject;
                string description = x.ContainsKey("description") ? x["description"].ToString() : "";
                string notes = x.ContainsKey("notes") ? x["notes"].ToString() : "";
                return new ConfigurationComposite(array: (JObject)(x["value"]), father: father, name: name, is_explicit: true, description: description, notes: notes);
            }
            return null;
        }

        public static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && !((JObject)fromJson).ContainsKey("type");
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson).ContainsKey("type") && ((JObject)fromJson)["type"].ToString().Equals("composite");
        }

        public override dynamic GetDictionaryToSerialize()
        {
            if (IsExplicit)
            {
                Dictionary<string, object> base_dict = base.GetDictionaryToSerialize() as Dictionary<string, object>;
                base_dict["type"] = "composite";
                base_dict["value"] = Variables.ToDictionary(x => x.ConfigurationName, x => x.GetDictionaryToSerialize());
                return base_dict;
            }
            return Variables.ToDictionary(x => x.ConfigurationName, x => x.GetDictionaryToSerialize());
        }

        public override string ToString()
        {
            return ShortPreview();
        }

        public override ConfigurationVariable Clone(Changable father = null)
        {
            return new ConfigurationComposite(this, father);
        }

        public ConfigurationVariable this[string key]
        {
            get => Variables.ToList().Find(x => x.ConfigurationName == key);
        }

        public override bool CheckDirty()
        {
            return dirty || Variables.Any(x => x.Dirty);
        }

        public override UserControl GetEditView()
        {
            return new ConfigurationVariableContent(this);
        }

        public override string ShortPreview()
        {
            string s = String.Join(", ", Variables.Select(x => x.ConfigurationName).ToList());
            if (s.Length > 12)
            {
                s = s.Substring(0, 12);
                
            }
            for (int i = 0; i < 3; i++)
            {
                if (!s.EndsWith("..."))
                {
                    s += ".";
                }
            }
            return s;
        }
    }
}
