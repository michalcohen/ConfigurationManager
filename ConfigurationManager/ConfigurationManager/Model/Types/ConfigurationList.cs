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
        public ConfigurationList(JArray array, string name="")
        {
            FontColor = Brushes.Black;
            ConfigurationName = name;
            Variables = new List<ConfigurationVariable>();
            foreach (JToken value in array)
            {
                Variables.Add(ConfigurationVariable.ConvertJsonToConfiguration("", value));
            }
        }

        public static new ConfigurationVariable TryConvert(string name, JToken fromJson)
        {
            if (fromJson.Type == JTokenType.Array)
            {
                return new ConfigurationList((JArray)fromJson, name);
            }
            return null;
        }

        public override object GetDictionary()
        {
            return new List<object>(Variables.Select(x => x.GetDictionary()));
        }

        public override bool IsDirty()
        {
            return Variables.Any<ConfigurationVariable>(v => v.IsDirty());
        }

        public new void Saved()
        {
            Dirty = false;
            foreach (ConfigurationVariable variable in Variables)
            {
                variable.Saved();
            }
        }

        public override string ToString()
        {
            return "";
        }

        public override Window GetGUIElementsForEdit()
        {
            return new Window();
        }
    }
}
