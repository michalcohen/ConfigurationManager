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
        public CompositeConfiguraionVariable(JObject array, Changable father = null, string name="") : base(father, Brushes.Black, name)
        {
            foreach (KeyValuePair<String, JToken> value in array)
            {
                Variables.Add(ConfigurationVariable.ConvertJsonToConfiguration(value.Key, value.Value, this));
            }
            IsComposite = true;
        }

        public CompositeConfiguraionVariable(CompositeConfiguraionVariable other): base(other)
        {}

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

        public override ConfigurationVariable Clone()
        {
            return new CompositeConfiguraionVariable(this);
        }

        public ConfigurationVariable this[string key]
        {
            get => Variables.ToList().Find(x => x.ConfigurationName == key);
        }

        public override bool CheckDirty()
        {
            return Variables.Any(x => x.Dirty);
        }
    }
}
