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
    public class ConfigurationList : ConfigurationVariable
    {
        private static Brush brush = Brushes.LightGray;
        public ConfigurationList(JArray array, Changable father = null, string name="", bool is_explicit = false, string description = "", string notes = "") : base(father, ConfigurationList.brush, name, is_explicit, description, notes)
        {
            int index = 1;
            foreach (JToken value in array)
            {
                ConfigurationVariable cv = ConfigurationVariable.ConvertJsonToConfiguration("Element " + index.ToString(), value, this);
                if (cv != null)
                {
                    Variables.Add(ConfigurationVariable.ConvertJsonToConfiguration("Element " + index.ToString(), value, this));
                    index++;
                }
            }
            IsComposite = true;
        }

        public ConfigurationList(ConfigurationList other, Changable father = null): base(other, father)
        {}

        public ConfigurationList() : base() 
        {
            FontColor = ConfigurationList.brush;
        }

        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationList(array: (JArray)fromJson, father: father, name: name);
            } else if (IsExplicitType(fromJson))
            {
                JObject x = fromJson as JObject;
                string description = x.ContainsKey("description") ? x["description"].ToString() : "";
                string notes = x.ContainsKey("notes") ? x["notes"].ToString() : "";
                return new ConfigurationList(array: (JArray)(x["value"]), father: father, name: name, is_explicit: true, description: description, notes: notes);
            }
            return null;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson).ContainsKey("type") && ((JObject)fromJson)["type"].ToString().Equals("list");
        }

        public static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Array;
        }

        public override ConfigurationVariable Clone(Changable father = null)
        {
            return new ConfigurationList(this, father);
        }

        public override dynamic GetDictionaryToSerialize()
        {
            var value = Variables.Select(x => x.GetDictionaryToSerialize()).ToList();
            if (IsExplicit)
            {
                Dictionary<string, object> base_dict = base.GetDictionaryToSerialize() as Dictionary<string, object>;
                base_dict["type"] = "list";
                base_dict["value"] = value;
                return base_dict;
            }
            return value;
        }

        public override void AddVariable(ConfigurationVariable configurationVariable)
        {
            configurationVariable.ConfigurationName = "Element " + (Variables.Count + 1).ToString();
            base.AddVariable(configurationVariable);
        }

        protected override void DeleteVariable(ConfigurationVariable configurationVariable)
        {
            int index = Variables.IndexOf(configurationVariable);
            for (int i = index + 1; i < Variables.Count; i++)
            {
                Variables[i].ConfigurationName = "Element " + i.ToString();
            }
            base.DeleteVariable(configurationVariable);
        }

        public override string ShortPreview()
        {
            string s = String.Join(", ", Variables.Select(x => x.ShortPreview()).ToList());
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

        public override bool CheckValidity()
        {
            return Variables.All(x => x.IsValid);
        }

        public override string ToString()
        {
            return ShortPreview();
        }

        public override bool CheckDirty()
        {
            return dirty || Variables.Any(x => x.Dirty);
        }

        public override UserControl GetEditView()
        {
            return new ConfigurationVariableContent(this);
        }

    }
}
