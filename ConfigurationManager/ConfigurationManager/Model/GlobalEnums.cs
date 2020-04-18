using Newtonsoft.Json;
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

        private GlobalEnums()
        {
            EnumsOptions = new ObservableCollection<GlobalEnum>();
        }

        private void AddEnum(string name, JArray values)
        {
            EnumsOptions.Add(new GlobalEnum(name, values));
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

        /// <summary>
        /// Checks if a given value is one of the option stated by the definition of enumName in EnumOption.
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ValidValue(string enumName, string value)
        {
            return HasEnum(enumName) && instance.EnumsOptions.Where(x => x.Name.Equals(enumName)).First().Options.Contains(value);
        }

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                PropertyChanged(this, new PropertyChangedEventArgs("EnumsOptions"));
            }
        }
    }

    public class GlobalEnum: INotifyPropertyChanged{

        public string Name { get; set; }
        public ObservableCollection<string> Options { get; set; }

        public GlobalEnum(string name, JArray values)
        {
            Name = name;
            Options = new ObservableCollection<string>(values.Values<string>());
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
            Options.Add(text);
            RaisePropertyChanged("Options");
        }
    }
}
