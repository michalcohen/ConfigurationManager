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
        public ConfigurationComposite(JObject array, Changable father = null, string name="") : base(father, ConfigurationComposite.brush, name)
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

        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsRelevantType(fromJson))
            {
                return new ConfigurationComposite((JObject)fromJson, father, name);
            }
            return null;
        }

        public static bool IsRelevantType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && !((JObject)fromJson).ContainsKey("type");
        }

        public override object GetDictionary()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (ConfigurationVariable variable in Variables)
            {
                dict[variable.ConfigurationName] = variable.GetDictionary();
            }
            return dict;
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
