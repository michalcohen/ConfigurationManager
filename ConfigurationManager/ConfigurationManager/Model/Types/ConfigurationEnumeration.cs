using ConfigurationManager.View.UserControls.EditValuesWindows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class EnumType: InnerType<string>
    {
        private bool _is_global_enum;
        public bool IsGlobalEnum 
        { 
            get
            {
                return _is_global_enum;
            }
            set
            {
                if (value != _is_global_enum)
                {
                    _is_global_enum = value;
                    RaisePropertyChanged("IsGlobalEnum");
                    RaisePropertyChanged("IsLocalEnum");
                    RaisePropertyChanged("Options");
                    Dirty = true;
                }
            }
        }
        public bool IsLocalEnum
        {
            get
            {
                return !_is_global_enum;
            }
        }

        private string _enum_name;
        public string EnumName 
        { get 
            { 
                return _enum_name; 
            } 
          set
            {
                if (value != _enum_name)
                {
                    _enum_name = value;
                    RaisePropertyChanged("TextRepresentation");
                    RaisePropertyChanged("Options");
                    Dirty = true;
                }
            }
        }

        public ObservableCollection<string> EnumValues { get; set; }

        public List<string> Options
        {
            get
            {
                if (IsGlobalEnum)
                {
                    return GlobalEnums.GetGlobalEnum(EnumName).Values;
                }
                return new List<string>(EnumValues);
            }
        }
        
        public EnumType(ConfigurationEnumeration father, string value, string enum_name): base(father, value, true)
        {
            _enum_name = enum_name;
            _is_global_enum = true;
            EnumValues = new ObservableCollection<string>();
        }

        public EnumType(ConfigurationEnumeration father, string value, List<string> enum_values): base(father, value, true)
        {
            EnumValues = new ObservableCollection<string>(enum_values);
            _is_global_enum = false;
        }

        public EnumType(EnumType other, ConfigurationVariable father = null) : base(other, father)
        {
            _is_global_enum = other._is_global_enum;
            _enum_name = other._enum_name;
            EnumValues = new ObservableCollection<string>(other.EnumValues);
        }

        public override object GetDictionary()
        {
            if (IsGlobalEnum)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    ["type"] = EnumName,
                    ["value"] = Value
                };
                return dict;
            }
            else
            {
                Dictionary<string, object> dict = new Dictionary<string, object>
                {
                    ["type"] = EnumValues,
                    ["value"] = Value
                };
                return dict;
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override InnerType<string> Clone(ConfigurationVariable father = null)
        {
            return new EnumType(this, father);
        }

        public override void UpdateBy(InnerType<string> other)
        {
            EnumType o = other as EnumType;
            base.UpdateBy(o);
            IsGlobalEnum = o.IsGlobalEnum;
            EnumName = o.EnumName;
            EnumValues = o.EnumValues;

        }

        public void AddLocalEnumOption(string new_option)
        {
            EnumValues.Add(new_option);
            RaisePropertyChanged("EnumValues");
            RaisePropertyChanged("Options");
            Dirty = true;
        }

        public void RemoveLocalEnumOption(string new_option)
        {
            EnumValues.Remove(new_option);
            RaisePropertyChanged("EnumValues");
            RaisePropertyChanged("Options");
            Dirty = true;
        }
    }
    
    public class ConfigurationEnumeration: ConfigurationVariable<EnumType, string>
    {
        private static Brush brush = Brushes.DarkGoldenrod;
        public ConfigurationEnumeration(string value, Changable father = null, string enum_name = "", string name="") : base(father, ConfigurationEnumeration.brush, name, true)
        {
            Value = new EnumType(this, value, enum_name);
            IsExplicitnessChangeable = false;
        }

        public ConfigurationEnumeration(string value, Changable father = null, List<string> enum_values = null, string name="") : base(father, Brushes.DarkGoldenrod, name, true)
        {
            Value = new EnumType(this, value, enum_values);
            IsExplicitnessChangeable = false;
        }

        public ConfigurationEnumeration(ConfigurationEnumeration other, Changable father = null): base(other, father)
        {}

        public ConfigurationEnumeration(): base()
        {
            FontColor = ConfigurationEnumeration.brush;
            Value = new EnumType(this, "", new List<string>() { "" });
        }
        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
        {
            if (fromJson.Type != JTokenType.Object)
            {
                return null;
            }
            JObject j = (JObject)fromJson;
            if (!j.ContainsKey("type"))
            {
                return null;
            }
            JToken t = j["type"];
            if (t.Type == JTokenType.String)
            {
                string type_name = t.ToObject<string>();
                if (GlobalEnums.HasEnum(type_name))
                {
                    return new ConfigurationEnumeration(j["value"].ToObject<string>(), father, type_name, name);
                }
            }
            else if (t.Type == JTokenType.Array)
            {
                JArray vals = (JArray)t;
                if (vals[0].Type != JTokenType.String)
                {
                    return null;
                }
                List<string> values = new List<string>();
                foreach (JToken value in vals)
                {
                    values.Add(value.ToObject<string>());
                }
                return new ConfigurationEnumeration(j["value"].ToObject<string>(), father, values, name);
            }
            return null;
        }

        public override UserControl GetEditView()
        {
            return new EditEnum(Value);
        }

        public override ConfigurationVariable Clone(Changable father = null)
        {
            return new ConfigurationEnumeration(this, father);
        }

        public void UpdateBy(ConfigurationEnumeration other)
        {
            base.UpdateBy(other);
        }
    }
}
