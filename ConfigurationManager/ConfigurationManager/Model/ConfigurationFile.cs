using ConfigurationManager.Model.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ConfigurationManager
{
    public class ConfigurationFile
    {
        public CompositeConfiguraionVariable Content { get; }

        public string AllNames { get
            {
                return String.Join(" ", Content.AllNames);
            } 
        }


        public ConfigurationFile(JObject array)
        {
            Content= new CompositeConfiguraionVariable(array);
        }

        public Dictionary<string, object> GetDictionary()
        {
            return Content.GetDictionary() as Dictionary<string, object>;
        }

        internal void Saved()
        {
            Content.Saved();
        }

        internal bool IsDirty
        {
            get
            {
                return Content.IsDirty();
            }
        }
    }
}
