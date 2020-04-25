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
        private static Brush brush = Brushes.Black;
        public ConfigurationList(JArray array, Changable father = null, string name="") : base(father, ConfigurationList.brush, name)
        {
            foreach (JToken value in array)
            {
                Variables.Add(ConfigurationVariable.ConvertJsonToConfiguration("", value, this));
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
            if (fromJson.Type == JTokenType.Array)
            {
                return new ConfigurationList((JArray)fromJson, father, name);
            }
            return null;
        }

        public override ConfigurationVariable Clone(Changable father = null)
        {
            return new ConfigurationList(this, father);
        }

        public override object GetDictionary()
        {
            return new List<object>(Variables.Select(x => x.GetDictionary()));
        }

        public override string ToString()
        {
            return "";
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
