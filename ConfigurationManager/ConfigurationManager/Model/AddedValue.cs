using ConfigurationManager.Model.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ConfigurationManager.Model
{
    class AddedValue : INotifyPropertyChanged
    {
        public ConfigurationVariable current_value { get; set; }
        public List<string> PossibleTypesNames 
        { 
            get
            {
                return PossibleTypes.Select(x => x.Name.Substring(13)).ToList();
            } 
        }

        public List<Type> PossibleTypes
        {
            get
            {
                return ConfigurationVariable.GetAllConfigurationTypes();
            }
        }

        private string _current_type;

        public event PropertyChangedEventHandler PropertyChanged;
        bool is_name_visible;

        public string CurrentType
        {
            get
            {
                return _current_type;
            }
            set
            {
                if (!value.Equals(_current_type))
                {
                    _current_type = value;
                    current_value = PossibleTypes.Where(x => x.Name.Contains(_current_type)).First().GetConstructor(new Type[0]).Invoke(new object[0]) as ConfigurationVariable;
                    current_value.IsNameVisable = is_name_visible;
                    RaisePropertyChanged("CurrentType");
                    RaisePropertyChanged("current_value");
                }
            }
        }

        public AddedValue(ConfigurationVariable cv)
        {
            if (!(cv is ConfigurationList || cv is ConfigurationComposite))
            {
                throw new Exception("whattt");
            }
            if (cv.Variables.Count > 0)
            {
                current_value = cv.Variables.First().GetType().GetConstructor(new Type[0]).Invoke(new Type[0]) as ConfigurationVariable;
            } else
            {
                current_value = new ConfigurationInteger();
            }
            current_value.IsNameVisable = cv is ConfigurationComposite;
            is_name_visible = current_value.IsNameVisable;
            _current_type = current_value.GetType().Name.Substring(13);
            
        }

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));

            }
        }
    }
}
