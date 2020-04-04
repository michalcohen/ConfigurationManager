using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationManager.Types
{
    abstract public class ConfigurationVariable
    {
        abstract public bool IsValidValue(Object o);

        public static ConfigurationVariable TryConvert(JToken fromJson) => throw new NotImplementedException();

        private static List<Type> GetAllConfigurationTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(ConfigurationVariable).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
        }

        public static ConfigurationVariable ConvertJsonToConfiguration(JToken fromJson)
        {
            Type relevant_type = GetAllConfigurationTypes().Find(t => (bool)t.GetMethod("IsRelevantType").Invoke(null, new object[] { fromJson }));
            return (ConfigurationVariable)relevant_type.GetMethod("TryConvert").Invoke(null, new object[] { fromJson });
        }

        public static bool IsImplicitType(JToken fromJson) => throw new NotImplementedException();

        public static bool IsExplicitType(JToken fromJson) => throw new NotImplementedException();

        public static bool IsRelevantType(JToken fromJson)
        {
            return IsImplicitType(fromJson) || IsExplicitType(fromJson);
        }
    }
}
