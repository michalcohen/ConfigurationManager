﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager.Types
{
    public class CompositeConfiguraionVariable: ConfigurationVariable
    {
        public Dictionary<String, ConfigurationVariable> Variables { get; set; }

        public CompositeConfiguraionVariable(JObject array)
        {
            Variables = new Dictionary<String, ConfigurationVariable>();
            foreach (KeyValuePair<String, JToken> value in array)
            {
                Variables[value.Key] = ConfigurationVariable.ConvertJsonToConfiguration(value.Value);
            }
        }

        public static new ConfigurationVariable TryConvert(JToken fromJson)
        {
            if (IsRelevantType(fromJson))
            {
                return new CompositeConfiguraionVariable((JObject)fromJson);
            }
            return null;
        }

        public static bool IsRelevantType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && !((JObject)fromJson).ContainsKey("type");
        }

        public override bool IsValidValue(object o)
        {
            throw new NotImplementedException();
        }
    }
}
