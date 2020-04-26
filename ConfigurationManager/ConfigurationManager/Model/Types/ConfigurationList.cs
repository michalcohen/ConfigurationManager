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
        public ConfigurationList(JArray array, Changable father = null, string name="") : base(father, ConfigurationList.brush, name)
        {
            int index = 1;
            foreach (JToken value in array)
            {
                Variables.Add(ConfigurationVariable.ConvertJsonToConfiguration("Element " + index.ToString(), value, this));
                index++;
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
