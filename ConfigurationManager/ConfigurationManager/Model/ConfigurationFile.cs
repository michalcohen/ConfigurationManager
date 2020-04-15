using ConfigurationManager.Model.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;
using ConfigurationManager.Model;

namespace ConfigurationManager
{
    public class ConfigurationFile: Changable
    {
        public CompositeConfiguraionVariable Content { get; }

        public string AllNames { get
            {
                return String.Join(" ", Content.AllNames);
            } 
        }


        public ConfigurationFile(JObject array)
        {
            Content = new CompositeConfiguraionVariable(array, this);
        }

        public Dictionary<string, object> GetDictionary()
        {
            return Content.GetDictionary() as Dictionary<string, object>;
        }

        internal void Saved()
        {
            Content.Saved();
        }

        public void Changed(string property)
        {
            
        }

        internal bool IsDirty
        {
            get
            {
                return Content.Dirty;
            }
        }
    }
}
