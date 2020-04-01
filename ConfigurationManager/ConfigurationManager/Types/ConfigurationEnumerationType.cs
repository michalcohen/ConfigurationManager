using ConfigurationManager.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationManager
{
    class ConfigurationEnumerationType
    {
        private String _name;
        private List<String> _values;
        private bool _isValid;

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    // OnPropertyChanged();
                    //SetIsValid();
                }
            }
        }

        public List<String> Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }
        }

        /// <summary>
        /// Indicates whether the model is in a valid state or not.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    //OnPropertyChanged();
                }
            }
        }

        private void SetIsValid()
        {
            IsValid = !string.IsNullOrEmpty(Name);
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        //private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public bool IsValidValue(object o)
        {
            string s = (string)o;
            return Values.Contains(s);
        }
    }
}
