﻿using ConfigurationManager.View.UserControls.EditValuesWindows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{
    public class IntegerType: BoundedInnerType<int>
    {
        public IntegerType(ConfigurationInteger father, int value, int lowest, int highest, bool is_explicit) : base(father, value, lowest, highest, is_explicit)
        {}

        public IntegerType(IntegerType other, ConfigurationVariable father = null) : base(other, father) { }

        public override InnerType<int> Clone(ConfigurationVariable father = null)
        {
            return new IntegerType(this, father);
        }
    }
    public class ConfigurationInteger: ConfigurationVariable<IntegerType, int>
    {
        private static Brush brush = Brushes.Green;
        public ConfigurationInteger(int val, Changable father = null, bool is_explicit = false, int lowest=int.MinValue, int highest = int.MaxValue, string name="") : base(father, ConfigurationInteger.brush, name, is_explicit)
        {
            Value = new IntegerType(this, val, lowest, highest, is_explicit);
        }

        public ConfigurationInteger(ConfigurationInteger other, Changable father = null): base(other, father)
        {}

        public ConfigurationInteger(): base()
        {
            FontColor = ConfigurationInteger.brush;
            Value = new IntegerType(this, 0, int.MinValue, int.MaxValue, true);
        }

        public override UserControl GetEditView()
        {
            return new EditInteger(Value);
        }

        public static new ConfigurationInteger TryConvert(string name, JToken fromJson, Changable father)
        {
            if (IsImplicitType(fromJson))
            {
                return new ConfigurationInteger(fromJson.ToObject<int>(), father, false, name: name);
            } else if (IsExplicitType(fromJson))
            {
                JObject j = (JObject)fromJson;
                int l = j.ContainsKey("lower_bound") ? j["lower_bound"].ToObject<int>() : int.MinValue;
                int h = j.ContainsKey("higher_bound") ? j["higher_bound"].ToObject<int>() : int.MaxValue;
                return new ConfigurationInteger(fromJson["value"].ToObject<int>(), father, true, lowest: l, highest: h, name:name);
            }
            return null;
        }

        private static new bool IsImplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Integer;
        }

        public static new bool IsExplicitType(JToken fromJson)
        {
            return fromJson.Type == JTokenType.Object && ((JObject)fromJson)["type"].ToString().Equals("int");
        }

        public override ConfigurationVariable Clone(Changable father = null)
        {
            return new ConfigurationInteger(this, father);
        }
    }
}
