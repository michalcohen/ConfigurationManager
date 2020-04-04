using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class ConfigurationList : ConfigurationVariable
    {
        public List<ConfigurationVariable> Variables { get; set; }

        public ConfigurationList(JArray array)
        {
            Variables = new List<ConfigurationVariable>();
            foreach (JToken value in array)
            {
                Variables.Add(ConfigurationVariable.ConvertJsonToConfiguration(value));
            }
        }

        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            return new ConfigurationList((JArray)fromJson);
        }

        public static new bool IsRelevantType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Array;
        }

        public override bool IsValidValue(object o)
        {
            throw new NotImplementedException();
        }
    }
}
