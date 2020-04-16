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
        /// <summary>
        /// Every configuration file is surounded by composite element.
        /// For example, each json file always surounded by { }. 
        /// Therefor, the root element of each configuration file is CompositeConfigurationVariable.
        /// </summary>
        public CompositeConfiguraionVariable Content { get; }

        public ConfigurationFile(JObject array)
        {
            Content = new CompositeConfiguraionVariable(array, this);
        }

        /// <summary>
        /// Return a dictionary describing the content of the file. This dictionary is in use
        /// when saving file to the disk.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetDictionary()
        {
            return Content.GetDictionary() as Dictionary<string, object>;
        }

        /// <summary>
        /// Notify all configuration variables they were saved. Any dirty flag still on will be set "false".
        /// </summary>
        internal void Saved()
        {
            Content.Saved();
        }

        /// <summary>
        /// "Changed" is called whenever one of the configuration variables is chaned.
        /// </summary>
        /// <param name="property"></param>
        public void Changed(string property)
        {
            
        }

        /// <summary>
        /// A file is dirty if any of its configuration variables are dirty.
        /// </summary>
        internal bool IsDirty
        {
            get
            {
                return Content.Dirty;
            }
        }
    }
}
