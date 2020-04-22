using ConfigurationManager.View.UserControls.EditValuesWindows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class FloatType: BoundedInnerType<float>
    {
        public FloatType(float value, float lowest, float highest, bool is_explicit) : base(value, lowest, highest, is_explicit)
        {}
    }
    public class ConfigurationFloat: ConfigurationVariable<float>
    {
        public ConfigurationFloat(float val, Changable father = null, bool is_explicit = false, float lowest=float.MinValue, float highest=float.MaxValue, string name="") : base(father, Brushes.BlueViolet, name, is_explicit)
        {
            if (lowest > highest)
            {
                throw new Exception("Invlid bounderies");
            }
            Value = new FloatType(val, lowest, highest, is_explicit);
        }

        public void Update(float newValue, float newLowest, float newHighest)
        {
            FloatType toUpdate = Value as FloatType;

            if (!newValue.Equals(toUpdate.Value))
            {
                toUpdate.Value = newValue;
                Dirty = true;
            }
            if (!newLowest.Equals(toUpdate.LowestValue))
            {
                toUpdate.LowestValue = newLowest;
                Dirty = true;
            }
            if (!newHighest.Equals(toUpdate.HighestValue))
            {
                toUpdate.HighestValue = newHighest;
                Dirty = true;
            }

            if (Dirty)
            {
                RaisePropertyChanged("TextRepresentation");
            }
        }

        public override UserControl GetEditView()
        {
            return new EditFloat(this);
        }
        public static new ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationFloat(fromJson.ToObject<float>(), father, false, name: name);
            } else if (IsExplicitType(fromJson))
            {
                JObject j = (JObject)fromJson;
                float l = j.ContainsKey("lower_bound") ? j["lower_bound"].ToObject<float>() : float.MinValue;
                float h = j.ContainsKey("higher_bound") ? j["higher_bound"].ToObject<float>() : float.MaxValue;
                return new ConfigurationFloat(fromJson["value"].ToObject<float>(), father, true, lowest: l, highest: h, name:name);
            }
            return null;
        }

        private static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Float;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson)["type"].ToString().Equals("float");
        }
    }
}
