using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
                    RaisePropertyChanged("IsValid");
                    RaisePropertyChanged("Value");
                    Dirty = true;
                }
            }
        }

        public ConfigurationVariable Father;

        public bool Dirty { get; set; }

        public bool IsExplicit { get; set; }

        public InnerType(ConfigurationVariable father, T value, bool is_explicit = false)
        {
            Father = father;
            Value = value;
            Dirty = false;
        }

        public InnerType(InnerType<T> other, ConfigurationVariable father)
        {
            Father = father;
            Value = other.Value;
            Dirty = other.Dirty;
        }

        public virtual void UpdateBy(InnerType<T> other)
        {
            Value = other.value_field;
            RaisePropertyChanged("IsValid");
        }

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                
            }
            Father.Changed(property);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public virtual dynamic GetObjectToSerialize(bool is_explicit)
        {
            IsExplicit = is_explicit;
            Dictionary<string, object> d = new Dictionary<string, object>();
            if (IsExplicit)
            {
                d["type"] = String.Join("", typeof(T).Name.ToLower().Where(x => x >= 'a' && x <= 'z').ToList());
                d["value"] = Value;
                return d;
            }
            return Value;
        }

        public abstract InnerType<T> Clone(ConfigurationVariable father = null);

        public void Saved()
        {
            Dirty = false;
        }

        public virtual bool CheckValidity()
        {
            return true;
        }
    }

    public abstract class BoundedInnerType<T> : InnerType<T> where T : IComparable
    {
        T minValue;
        T maxValue;
        
        public BoundedInnerType(ConfigurationVariable father, T value, T lower_bound, T higher_bound,
            bool is_low_bound, bool is_high_bound, bool is_explicit = false) : base(father, value, is_explicit)
        {
            _lowest_value = lower_bound;
            _highest_value = higher_bound;
            maxValue = (T)(typeof(T).GetField("MaxValue", BindingFlags.Public | BindingFlags.Static)).GetValue(null);
            minValue = (T)(typeof(T).GetField("MinValue", BindingFlags.Public | BindingFlags.Static)).GetValue(null);
            _is_high_bound = is_low_bound;
            _is_low_bound = is_high_bound;
        }

        public BoundedInnerType(BoundedInnerType<T> other, ConfigurationVariable father) : base(other, father)
        {
            _lowest_value = other._lowest_value;
            _highest_value = other._highest_value;
            minValue = other.minValue;
            maxValue = other.maxValue;
            _is_low_bound = other._is_low_bound;
            _is_high_bound = other._is_high_bound;
        }

        public override void UpdateBy(InnerType<T> other)
        {
            BoundedInnerType<T> o = other as BoundedInnerType<T>;
            base.UpdateBy(o);
            LowestValue = o.LowestValue;
            HighestValue = o.HighestValue;
            IsLowBound = o.IsLowBound;
            IsHighBound = o.IsHighBound;
        }

        private T _lowest_value;
        public T LowestValue 
        {
            get
            {
                return _lowest_value;
            }
            set 
            { 
                if (!value.Equals(_lowest_value))
                {
                    _lowest_value = value;
                    RaisePropertyChanged("LowestValue");
                    Dirty = true;
                }
            } 
        }

        private T _highest_value;
        public T HighestValue 
        {
            get 
            {
                return _highest_value;
            } 
            set 
            { 
                if (!value.Equals(_highest_value))
                {
                    _highest_value = value;
                    RaisePropertyChanged("TextRepresentation");
                    Father.Changed("TextRepresentation");
                    Dirty = true;
                }
            }
        }

        private bool _is_low_bound;
        public bool IsLowBound
        {
            get
            {
                return _is_low_bound;
            }
            set
            {
                if (!value.Equals(_is_low_bound))
                {
                    _is_low_bound = value;
                    RaisePropertyChanged("IsLowBound");
                    Dirty = true;
                }
            }
        }

        private bool _is_high_bound;
        public bool IsHighBound
        {
            get
            {
                return _is_high_bound;
            }
            set
            {
                if (!value.Equals(_is_high_bound))
                {
                    _is_high_bound = value;
                    RaisePropertyChanged("IsHighBound");
                    Dirty = true;
                }
            }
        }

        public override string ToString()
        {
            string ls = LowestValue.Equals(minValue) ? "-Inf" : LowestValue.ToString();
            string hs = HighestValue.Equals(maxValue) ? "Inf" : HighestValue.ToString();
            return Value.ToString() + " [" + ls + ", " + hs + "]";
        }

        public override dynamic GetObjectToSerialize(bool is_explicit)
        {
            IsExplicit = is_explicit;
            if (IsExplicit)
            {
                Dictionary<string, object> dict = base.GetObjectToSerialize(is_explicit) as Dictionary<string, object>;
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

        public override bool CheckValidity()
        {
            return Value.CompareTo(LowestValue) >= 0 && Value.CompareTo(HighestValue) <= 0;
        }
    }
}
