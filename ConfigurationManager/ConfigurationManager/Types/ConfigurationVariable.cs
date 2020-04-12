using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ConfigurationManager.Types
{

    abstract public class ConfigurationVariable
    {
        protected bool Dirty { get; set; }

        private static List<Type> GetAllConfigurationTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(ConfigurationVariable).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
        }

        public static ConfigurationVariable ConvertJsonToConfiguration(JToken fromJson)
        {
            return GetAllConfigurationTypes().Select(t => t.GetMethod("TryConvert").Invoke(null, new object[] { fromJson })).First(o => o != null) as ConfigurationVariable;
        }

        public abstract bool IsDirty();

        public void Saved()
        {
            Dirty = false;
        }

        public abstract object GetDictionary();

        public static bool IsImplicitType(JToken fromJson) => throw new NotImplementedException();

        public static bool IsExplicitType(JToken fromJson) => throw new NotImplementedException();

        public static ConfigurationVariable TryConvert(JToken fromJson) => throw new NotImplementedException();

        public abstract Brush GetFontColor();
    }

    abstract public class ConfigurationVariable<T>: ConfigurationVariable
    {
        public static new ConfigurationVariable<T> TryConvert(JToken fromJson) => throw new NotImplementedException();

        public abstract void Update(T new_value);
    }
}
