using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigurationManager
{
    /// <summary>
    /// This static class contains all the known global enums of the configuration project.
    /// If the user defines an enum with enum type which is just string, then the type of the enum is searched here,
    /// and its values derives from EnumsOptions fiesld.
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Connects between enum type name and its possible values.
        /// </summary>
        public static Dictionary<string, List<string>> EnumsOptions = new Dictionary<string, List<string>>();
        
        private static void AddEnum(string name, JArray values)
        {
            EnumsOptions[name] = new List<string>(values.Values<string>());
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
                AddEnum(value.Key, (JArray)value.Value);
            }
        }

        /// <summary>
        /// Checks if enum type name exists in EnumOptions
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasEnum(string name)
        {
            return EnumsOptions.ContainsKey(name);
        }

        /// <summary>
        /// Checks if a given value is one of the option stated by the definition of enumName in EnumOption.
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ValidValue(string enumName, string value)
        {
            return EnumsOptions.ContainsKey(enumName) && EnumsOptions[enumName].Contains(value);
        }
    }
}
