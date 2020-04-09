﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConfigurationManager
{
    public class ConfigurationModel
    {
        Dictionary<string, ConfigurationFile> all_files;
        List<string> opened_files;

        string RootPath { get; set; }

        public ConfigurationModel(string root_path)
        {
            all_files = new Dictionary<string, ConfigurationFile>();
            LoadeJsons(root_path);
            opened_files = new List<string>();
        }

        private void LoadeJsons(string root_path)
        {
            RootPath = root_path;
            Enums.LoadEnums(RootPath);
            foreach (string file in Directory.GetFiles(RootPath, "*.json", SearchOption.AllDirectories))
            {
                if (file.Equals(RootPath + "\\Enums.json"))
                {
                    continue;
                }
                using StreamReader r = new StreamReader(file);
                string json = r.ReadToEnd();
                JObject array = (JObject)JsonConvert.DeserializeObject(json);
                ConfigurationFile c = new ConfigurationFile(array);
                all_files[file] = c;
            }
        }

        public void Save()
        {
            foreach (KeyValuePair<string, ConfigurationFile> v in all_files)
            {
                System.IO.File.WriteAllText(v.Key, JsonConvert.SerializeObject(v.Value.GetDictionary(), Formatting.Indented));
                v.Value.Saved();
            }
        }

        internal bool IsDirty()
        {
            return all_files.Values.Any<ConfigurationFile>(f => f.IsDirty());
        }

        public void AddOpenedFile(string path)
        {
            if (!opened_files.Contains(path))
            {
                opened_files.Add(path);
            }
        }

        public ConfigurationFile GetConfigurationFile(string path)
        {
            return all_files[path];
        }

        public bool IsFileOpened(string path)
        {
            return opened_files.Contains(path);
        }

        public void CloseFile(string path)
        {
            if (opened_files.Contains(path))
            {
                opened_files.Remove(path);
            }
        }
    }
}
