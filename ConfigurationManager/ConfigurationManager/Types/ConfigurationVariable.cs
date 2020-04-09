using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationManager.Types
{
    abstract public class ConfigurationVariable
    {
        protected bool Dirty { get; set; }
        abstract public bool IsValidValue(Object o);

        public static ConfigurationVariable TryConvert(JToken fromJson) => throw new NotImplementedException();

        private static List<Type> GetAllConfigurationTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(ConfigurationVariable).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
        }

        public static ConfigurationVariable ConvertJsonToConfiguration(JToken fromJson)
        {
            return GetAllConfigurationTypes().
                Select<Type, ConfigurationVariable>(t => (ConfigurationVariable)t.GetMethod("TryConvert").Invoke(null, new object[] { fromJson })).
                First<ConfigurationVariable>(o => o != null);
        }

        public static bool IsImplicitType(JToken fromJson) => throw new NotImplementedException();

        public static bool IsExplicitType(JToken fromJson) => throw new NotImplementedException();

        public abstract object GetDictionary();

        public void Saved()
        {
            Dirty = false;
        }

        public abstract bool IsDirty();

    }
}
