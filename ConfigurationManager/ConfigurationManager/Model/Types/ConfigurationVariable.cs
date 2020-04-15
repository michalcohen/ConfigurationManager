using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{

    abstract public class ConfigurationVariable: INotifyPropertyChanged
    {
        public List<ConfigurationVariable> Variables { get; set; }
        protected bool Dirty { get; set; }

        public string ConfigurationName { get; set; }

        public string LabelName
        {
            get
            {
                return ConfigurationName.Equals("") ? "" : ConfigurationName + ": ";
            }
        }

        public string TextRepresentation { get
            {
                return ToString();
            } }

        public Brush FontColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private static List<Type> GetAllConfigurationTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(ConfigurationVariable).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
        }

        public static ConfigurationVariable ConvertJsonToConfiguration(string name, JToken fromJson)
        {
            return GetAllConfigurationTypes().Select(t => t.GetMethod("TryConvert").Invoke(null, new object[] { name, fromJson })).First(o => o != null) as ConfigurationVariable;
        }

        public abstract bool IsDirty();

        public void Saved()
        {
            Dirty = false;
        }

        public abstract object GetDictionary();

        public static bool IsImplicitType(JToken fromJson) => throw new NotImplementedException();

        public static bool IsExplicitType(JToken fromJson) => throw new NotImplementedException();

        public static ConfigurationVariable TryConvert(string name, JToken fromJson) => throw new NotImplementedException();

        public abstract Window GetGUIElementsForEdit();

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    abstract public class ConfigurationVariable<T>: ConfigurationVariable
    {
        public static new ConfigurationVariable<T> TryConvert(JToken fromJson) => throw new NotImplementedException();

        public abstract void Update(T new_value);

    }
}
