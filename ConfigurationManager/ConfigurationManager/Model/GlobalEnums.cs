﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace ConfigurationManager.Model
{
    /// <summary>
    /// This static class contains all the known global enums of the configuration project.
    /// If the user defines an enum with enum type which is just string, then the type of the enum is searched here,
    /// and its values derives from EnumsOptions fiesld.
    /// </summary>
    public class GlobalEnums: INotifyPropertyChanged
    {
        private static GlobalEnums instance = new GlobalEnums();
        /// <summary>
        /// Connects between enum type name and its possible values.
        /// </summary>

        public ObservableCollection<GlobalEnum> EnumsOptions { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public bool Dirty;

        private GlobalEnums()
        {
            EnumsOptions = new ObservableCollection<GlobalEnum>();
            Dirty = false;
        }

        public void AddEnum(string name, JArray values = null)
        {
            EnumsOptions.Add(new GlobalEnum(name, values));
            Dirty = true;
        }

        public static void RemoveEnum(GlobalEnum ge)
        {
            instance.EnumsOptions.Remove(ge);
            instance.RaisePropertyChanged("EnumsOptions");
            instance.Dirty = true;
        }

        /// <summary>
        /// Loads all known enums from the file "Enums.json" in the highmost level of the project.
        /// </summary>
        /// <param name="root_path"></param>
        public static void LoadEnums(string root_path)
        {
            if (!File.Exists(root_path + "\\Enums.json")){
                return;
            }
            using StreamReader r = new StreamReader(root_path + "\\Enums.json");
            string json = r.ReadToEnd();
            foreach (KeyValuePair<String, JToken> value in (JObject)JsonConvert.DeserializeObject(json))
            {
                instance.AddEnum(value.Key, (JArray)value.Value);
            }
        }

        public static GlobalEnums GetIntance()
        {
            return instance;
        }

        /// <summary>
        /// Checks if enum type name exists in EnumOptions
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasEnum(string name)
        {
            return instance.EnumsOptions.Any(x => x.Name.Equals(name));
        }

        public static GlobalEnum GetGlobalEnum(string enumName)
        {
            return instance.EnumsOptions.Where(x => x.Name.Equals(enumName)).First();
        }

        /// <summary>
        /// Checks if a given value is one of the option stated by the definition of enumName in EnumOption.
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ValidValue(string enumName, string value)
        {
            return HasEnum(enumName) && GetGlobalEnum(enumName).ContainsValue(value);
        }

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                PropertyChanged(this, new PropertyChangedEventArgs("EnumsOptions"));
            }
        }

        public static bool IsDirty()
        {
            return instance.Dirty;
        }

        public static void Saved()
        {
            instance.Dirty = false;
        }

        internal static object GetDictionary()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (GlobalEnum ge in instance.EnumsOptions)
            {
                dict[ge.Name] = ge.GetDictionary();
            }
            return dict;
        }

        public static void Save(string root_path)
        {
            if (GlobalEnums.IsDirty())
            {
                System.IO.File.WriteAllText(root_path + "\\Enums.json", JsonConvert.SerializeObject(GlobalEnums.GetDictionary(), Formatting.Indented));
                GlobalEnums.Saved();
            }
        }
    }

    public class GlobalEnum: INotifyPropertyChanged{

        private string _name;
        public string Name 
        { 
            get 
            { 
                return _name; 
            } 
            set 
            { 
                if (value != _name) 
                { 
                    _name = value; 
                    GlobalEnums.GetIntance().Dirty = false; 
                } 
            } 
        }
        public ObservableCollection<GlobalEnumValue> Options { get; set; }

        public GlobalEnum(string name, JArray values = null)
        {
            Name = name;
            if (values != null)
            {
                Options = new ObservableCollection<GlobalEnumValue>(values.Values<string>().Select(x => new GlobalEnumValue(x, this)).ToList());
            } else
            {
                Options = new ObservableCollection<GlobalEnumValue>();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                GlobalEnums.GetIntance().RaisePropertyChanged("Options");
            }
        }

        internal void AddOption(string text)
        {
            Options.Add(new GlobalEnumValue(text, this));
            RaisePropertyChanged("Options");
        }

        internal bool ContainsValue(string value)
        {
            return Options.Contains(new GlobalEnumValue(value, this));
        }

        internal void Delete()
        {
            GlobalEnums.RemoveEnum(this);
        }

        internal object GetDictionary()
        {
            return Options.Select(x => x.Value).ToList();
        }
    }

    public class GlobalEnumValue : INotifyPropertyChanged
    {
        private string _value;
        public string Value 
        { 
            get 
            { 
                return _value; 
            } 
            set 
            { 
                if (value != _value) 
                {
                    _value = value;
                    RaisePropertyChanged("Value");
                    GlobalEnums.GetIntance().Dirty = false;
                } 
            } 
        }

        private GlobalEnum ContainingGlobalEnum;

        public GlobalEnumValue(string value, GlobalEnum ge)
        {
            _value = value;
            ContainingGlobalEnum = ge;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;


        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                //GlobalEnums.GetIntance().RaisePropertyChanged("Options");
            }
        }

        public override bool Equals(object obj)
        {
            return (obj is string && (obj as string).Equals(_value)) || (obj is GlobalEnumValue && (obj as GlobalEnumValue)._value.Equals(_value));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value);
        }

        public void Delete()
        {
            ContainingGlobalEnum.Options.Remove(this);
            ContainingGlobalEnum.RaisePropertyChanged("Options");
        }
    }
}