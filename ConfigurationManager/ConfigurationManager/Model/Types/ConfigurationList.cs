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
        public ConfigurationList(JArray array, Changable father = null, string name="") : base(father, Brushes.Black, name)
        {
            foreach (JToken value in array)
            {
                Variables.Add(ConfigurationVariable.ConvertJsonToConfiguration("", value, this));
            }
            IsComposite = true;
        }

        public ConfigurationList(ConfigurationList other): base(other)
        {}

        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
        {
            if (fromJson.Type == JTokenType.Array)
            {
                return new ConfigurationList((JArray)fromJson, father, name);
            }
            return null;
        }

        public override ConfigurationVariable Clone()
        {
            return new ConfigurationList(this);
        }

        public override object GetDictionary()
        {
            return new List<object>(Variables.Select(x => x.GetDictionary()));
        }

        public override string ToString()
        {
            return "";
        }

    }
}
