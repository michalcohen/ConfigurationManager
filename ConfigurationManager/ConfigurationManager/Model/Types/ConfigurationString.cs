using ConfigurationManager.GUIComponents.EditValuesWindows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class StringType: INotifyPropertyChanged
    {
        private string string_value = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public string Value { 
            get { return string_value; } 
            set { 
                if (!string_value.Equals(value))
                {
                    string_value = value;
                    RaisePropertyChanged("TextRepresentation");
                }
            } }

        public StringType(string value)
        {
            string_value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
    
    public class ConfigurationString: ConfigurationVariable<StringType>
    {
        bool IsExplicit { get; set; }

        public StringType Value { get; set; }

        public ConfigurationString(string val, Changable father, bool is_explicit = false, string name="")
        {

            Father = father;
            FontColor = Brushes.DarkKhaki;
            Value = new StringType(val);
            IsExplicit = is_explicit;
            ConfigurationName = name;
            Variables = new List<ConfigurationVariable>();
        }

        public void ChangeContent(string new_string)
        {
            if (!new_string.Equals(Value.Value))
            {
                Dirty = true;
                Value.Value = new_string;
                RaisePropertyChanged("TextRepresentation");
            }
            
        }
        
        public static new ConfigurationString TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationString(fromJson.ToObject<string>(), father, false, name);
            } else if (IsExplicitType(fromJson))
            {
                return new ConfigurationString(fromJson["value"].ToObject<string>(), father, true, name);
            }
            return null;
        }

        private static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.String;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson)["type"].ToString().Equals("string");
        }

        public override object GetDictionary()
        {
            if (IsExplicit)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["type"] = "string";
                dict["value"] = Value.Value;
                return dict;
            }
            return Value.Value;
        }

        public override void Update(StringType new_value)
        {
            Value = new_value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override Window GetGUIElementsForEdit()
        {
            return new StringEdit(this, ConfigurationName);
        }
    }
}
