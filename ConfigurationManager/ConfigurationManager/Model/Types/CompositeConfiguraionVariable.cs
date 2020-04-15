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
    public class CompositeConfiguraionVariable: ConfigurationVariable
    {
        public CompositeConfiguraionVariable(JObject array, Changable father, string name="")
        {
            Father = father;
            FontColor = Brushes.Black;
            ConfigurationName = name;
            Variables = new List<ConfigurationVariable>();
            foreach (KeyValuePair<String, JToken> value in array)
            {
                Variables.Add(ConfigurationVariable.ConvertJsonToConfiguration(value.Key, value.Value, this));
            }
        }

        public List<string> AllNames
        {
            get
            {
                return Variables.Select(v => v.ConfigurationName).ToList();
            }
        }

        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsRelevantType(fromJson))
            {
                return new CompositeConfiguraionVariable((JObject)fromJson, father, name);
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
            return "";
        }

        public ConfigurationVariable this[string key]
        {
            get => Variables.Find(x => x.ConfigurationName == key);
        }
        public override Window GetGUIElementsForEdit()
        {
            return new Window();
        }
    }
}
