﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ConfigurationManager.Model.Types
{
    public abstract class InnerType<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private T value_field;

        public T Value
        {
            get
            {
                return value_field;
            }

            set
            {
                if (value != null && !value.Equals(value_field))
                {
                    value_field = value;
                    RaisePropertyChanged("TextRepresentation");
                    Father.Changed("TextRepresentation");
                }
            }
        }

        public bool IsExplicit { get; set; }

        public InnerType(ConfigurationVariable father, T value, bool is_explicit = false)
        {
            Father = father;
            Value = value;
        }

        public InnerType(InnerType<T> other)
        {
            Father = other.Father;
            Value = other.Value;
        }

        public ConfigurationVariable Father;

        public virtual void UpdateBy(InnerType<T> other)
        {
            Value = other.value_field;
        }

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public virtual object GetDictionary()
        {
            if (IsExplicit)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict["type"] = typeof(T).Name;
                dict["value"] = Value;
                return dict;
            }
            return Value;
        }

        public abstract InnerType<T> Clone();
    }

    public abstract class BoundedInnerType<T> : InnerType<T> where T : IComparable
    {
        T minValue;
        T maxValue;
        public BoundedInnerType(ConfigurationVariable father, T value, T lower_bound, T higher_bound, bool is_explicit = false) : base(father, value, is_explicit)
        {
            LowestValue = lower_bound;
            HighestValue = higher_bound;
            maxValue = (T)(typeof(T).GetField("MaxValue", BindingFlags.Public | BindingFlags.Static)).GetValue(null);
            minValue = (T)(typeof(T).GetField("MinValue", BindingFlags.Public | BindingFlags.Static)).GetValue(null);
        }

        public BoundedInnerType(BoundedInnerType<T> other): base(other)
        {
            LowestValue = other.LowestValue;
            HighestValue = other.HighestValue;
            minValue = other.minValue;
            maxValue = other.maxValue;
        }

        public void UpdateBy(BoundedInnerType<T> other)
        {
            base.UpdateBy(other);
            LowestValue = other.LowestValue;
            HighestValue = other.HighestValue;
        }

        public T LowestValue { get; set; }

        public T HighestValue { get; set; }

        public override string ToString()
        {
            string ls = LowestValue.Equals(minValue) ? "-Inf" : LowestValue.ToString();
            string hs = HighestValue.Equals(maxValue) ? "Inf" : HighestValue.ToString();
            return Value.ToString() + " [" + ls + ", " + hs + "]";
        }

        public override object GetDictionary()
        {
            if (IsExplicit)
            {
                Dictionary<string, object> dict = base.GetDictionary() as Dictionary<string, object>;
                if (HighestValue.CompareTo(maxValue) < 0)
                {
                    dict["higher_bound"] = HighestValue;
                }
                if (LowestValue.CompareTo(minValue) > 0)
                {
                    dict["lower_bound"] = LowestValue;
                }
                return dict;
            }
            return Value;
        }
    }
}
