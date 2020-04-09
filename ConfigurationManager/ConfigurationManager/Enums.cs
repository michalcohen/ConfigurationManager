using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigurationManager
{
    public class Enums
    {
        public static Dictionary<string, List<string>> EnumsOptions = new Dictionary<string, List<string>>();
        
        private static void AddEnum(string name, JArray values)
        {
            EnumsOptions[name] = new List<string>(values.Values<string>());
        }

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

        public static bool HasEnum(string name)
        {
            return EnumsOptions.ContainsKey(name);
        }

        public static bool ValidValue(string enumName, string value)
        {
            return EnumsOptions.ContainsKey(enumName) && EnumsOptions[enumName].Contains(value);
        }
    }
}
