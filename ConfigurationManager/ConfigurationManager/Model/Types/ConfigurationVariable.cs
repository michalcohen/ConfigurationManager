using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{

    abstract public class ConfigurationVariable: INotifyPropertyChanged, Changable
    {
        public List<ConfigurationVariable> Variables { get; set; }
        
        public bool Dirty { get; set; }

        private string configuration_name;
        
        public string ConfigurationName { 
            get { return configuration_name; } 
            set {
                if (value != configuration_name)
                {
                    Dirty = true;
                    configuration_name = value;
                    RaisePropertyChanged("ConfigurationName");
                    RaisePropertyChanged("LabelName");
                }
            } }

        public string LabelName
        {
            get
            {
                return ConfigurationName.Equals("") ? "" : ConfigurationName + ": ";
            }
        }

        public Changable Father { get; set; }

        public string TextRepresentation { get
            {
                return ToString();
            } }

        public Brush FontColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        
        protected ConfigurationVariable(Changable father, Brush font_color, string name, Window edit_window=null)
        {
            Father = father;
            FontColor = font_color;
            ConfigurationName = name;
            Variables = new List<ConfigurationVariable>();
        }

        public virtual void OpenEditWindow()
        {
            new Window();
        }

        private static List<Type> GetAllConfigurationTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(ConfigurationVariable).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
        }

        public static ConfigurationVariable ConvertJsonToConfiguration(string name, JToken fromJson, Changable father)
        {
            return GetAllConfigurationTypes().Select(t => t.GetMethod("TryConvert").Invoke(null, new object[] { name, fromJson, father })).First(o => o != null) as ConfigurationVariable;
        }

        public void Saved()
        {
            Dirty = false;
            foreach (ConfigurationVariable cv in Variables)
            {
                cv.Saved();
            }
        }

        public abstract object GetDictionary();

        public static bool IsImplicitType(JToken fromJson) => throw new NotImplementedException();

        public static bool IsExplicitType(JToken fromJson) => throw new NotImplementedException();

        public static ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father) => throw new NotImplementedException();

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                Father.Changed(property);
            }
        }

        public void Changed(string property)
        {
            Father.Changed(property);
        }

        public bool IsNameNotChangeable
        {
            get
            {
                return ConfigurationName.Equals("");
            }
        }
    }

    abstract public class ConfigurationVariable<T> : ConfigurationVariable
    {
        public bool IsExplicit { get; set; }

        protected ConfigurationVariable(Changable father, Brush font_color, string name, bool is_explicit): base(father, font_color, name)
        {
            IsExplicit = is_explicit;
        }

        public static new ConfigurationVariable<T> TryConvert(JToken fromJson) => throw new NotImplementedException();

        public abstract void Update(T new_value);



    }
}
