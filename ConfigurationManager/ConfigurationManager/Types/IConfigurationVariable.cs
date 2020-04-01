using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationManager.Types
{
    interface IConfigurationVariable
    {
        bool IsValidValue(Object o);

        static IConfigurationVariable TryConvert(JToken fromJson) => throw new NotImplementedException();

        private static List<Type> GetAllConfigurationTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(IConfigurationVariable).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
        }

        static bool IsRelevantType(JToken fromJson) => throw new NotImplementedException();

        static IConfigurationVariable ConvertJsonToConfiguration(JToken fromJson)
        {
            Type relevant_type = GetAllConfigurationTypes().Find(t => (bool)t.GetMethod("IsRelevantType").Invoke(null, new object[] { fromJson }));
            return (IConfigurationVariable)relevant_type.GetMethod("TryConvert").Invoke(null, new object[] { fromJson });
        }
    }
}
