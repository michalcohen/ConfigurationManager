using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (fromJson.Type == JTokenType.Array)
            {
                return new ConfigurationList((JArray)fromJson);
            }
            return null;
        }

        public override object GetDictionary()
        {
            return new List<object>(Variables.Select(x => x.GetDictionary()));
        }

        public override bool IsDirty()
        {
            return Variables.Any<ConfigurationVariable>(v => v.IsDirty());
        }

        public new void Saved()
        {
            Dirty = false;
            foreach (ConfigurationVariable variable in Variables)
            {
                variable.Saved();
            }
        }
    }
}
