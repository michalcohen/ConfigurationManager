using ConfigurationManager.View.Windows;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ConfigurationManager.Model.Types
{

    abstract public class ConfigurationVariable: INotifyPropertyChanged, Changable
    {

        #region ConfigurationVariable Properties

        /// <summary>
        /// Containing subvariales for any composite configuration variable. like plain CompositeConfigurationVariable
        /// or ConfigurationList.
        /// </summary>
        public ObservableCollection<ConfigurationVariable> Variables { get; set; }

        /// <summary>
        /// True <=> any change was made to the configuration variable name or content that wasn't saved yet.
        /// </summary>
        public bool Dirty { get; set; }

        private string configuration_name;
        
        /// <summary>
        /// The name of the configuration variable. If the variable is annonimous (like element in list),
        /// ConfigurationName is empty string "".
        /// </summary>
        public string ConfigurationName { 
            get { return configuration_name; } 
            set {
                if (value != configuration_name)
                {
                    Dirty = true;
                    configuration_name = value;
                    RaisePropertyChanged("ConfigurationName");
                    RaisePropertyChanged("LabelName");
                }
            } }

        /// <summary>
        /// The representation of the configuration variable name as the label of the content.
        /// This property is binded to the view of ConfigurationFileContent
        /// </summary>
        public string LabelName
        {
            get
            {
                return ConfigurationName.Equals("") ? "" : ConfigurationName + ": ";
            }
        }

        /// <summary>
        /// Containing Configuration variable or configuration file to be notified whenever this variable
        /// is changed.
        /// </summary>
        public Changable Father { get; set; }

        /// <summary>
        /// The representation of the content as text.
        /// This property is binded to the view of ConfigurationFileContent
        /// </summary>
        public string TextRepresentation { get
            {
                return ToString();
            } }

        /// <summary>
        /// The color in which the representing text is colored.
        /// </summary>
        public Brush FontColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsComposite { get; set; }

        public UserControl EditWindow
        {
            get
            {
                return GetEditView();
            }
        }

        /// <summary>
        /// If the variable is annonimous (like element in list), then its name isan't changeable.
        /// </summary>
        public bool IsNameVisable
        {
            get
            {
                return !ConfigurationName.Equals("");
            }
        }

        protected bool is_explicit;
        /// <summary>
        /// True <=> the value was read as explicit type, or the user changed it to be explicit.
        /// </summary>
        public bool IsExplicit
        {
            get { return is_explicit; }
            set
            {
                if (value != is_explicit)
                {
                    is_explicit = value;
                }
            }
        }

        public bool IsImplicit
        {
            get
            {
                return !is_explicit;
            }
        }

        public bool IsExplicitnessChangeable { get; set; }

        #endregion


        #region ConfigurationVariable Constructors
        protected ConfigurationVariable(Changable father = null, Brush font_color = null, string name = "", Window edit_window=null)
        {
            Father = father;
            FontColor = font_color;
            ConfigurationName = name;
            Variables = new ObservableCollection<ConfigurationVariable>();
            Dirty = false;
            IsComposite = false;
            is_explicit = false;
            IsExplicitnessChangeable = false;
        }

        protected ConfigurationVariable(ConfigurationVariable other)
        {
            Father = new EmptyFather();
            UpdateBy(other);
        }

        class EmptyFather : Changable
        {
            public void Changed(string property)
            {
            }
        }
        #endregion


        #region ConfigurationVariable Methods

        public virtual void UpdateBy(ConfigurationVariable other)
        {
            IsExplicitnessChangeable = other.IsExplicitnessChangeable;
            IsExplicit = other.IsExplicit;
            IsComposite = other.IsComposite;
            PropertyChanged = other.PropertyChanged;
            FontColor = other.FontColor;
            ConfigurationName = other.ConfigurationName;
            Dirty = other.Dirty;
            Variables = new ObservableCollection<ConfigurationVariable>(other.Variables.Select(x => x.Clone()).ToList());
        }

        /// <summary>
        /// When the configuration variable is clicked in the ConfigurationFileContent view, an edit window is opened.
        /// Each configuration variable type has a different edit window.
        /// This function is momentary virtual until each configuration variable will implement it, then we will
        /// change this function to abstract, forcing future configuration variables to implement it.
        /// </summary>
        public void OpenEditWindow()
        {
            (new EditConfigurationVariable(this)).Show();
        }

        public virtual UserControl GetEditView()
        {
            throw new NotImplementedException();
        }

        internal void Delete()
        {
            if (Father is ConfigurationVariable)
            {
                (Father as ConfigurationVariable).DeleteSon(this);
            }
            else
            {
                MessageBox.Show("cannot delete highmost hierarchy of json file", "Configuration manager", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteSon(ConfigurationVariable configurationVariable)
        {
            Variables.Remove(configurationVariable);
            RaisePropertyChanged("Variables");
        }

        /// <summary>
        /// Returns using reflection all the ConfigurationVariable types.
        /// </summary>
        /// <returns></returns>
        private static List<Type> GetAllConfigurationTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(ConfigurationVariable).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
        }

        /// <summary>
        /// When loading file from the disk, each configuration variable needs to be casted to the right ConfigurationVariable
        /// type. We retrieve every ConfigurationVariable type using reflection and try to convert the loaded content to
        /// each of the types, until one of them successes converting the loaded data.
        /// </summary>
        /// <returns></returns>
        public static ConfigurationVariable ConvertJsonToConfiguration(string name, JToken fromJson, Changable father)
        {
            return GetAllConfigurationTypes().Select(t => t.GetMethod("TryConvert").Invoke(null, new object[] { name, fromJson, father })).First(o => o != null) as ConfigurationVariable;
        }

        /// <summary>
        /// Set dirty false and notify any sub variable that it was saved.
        /// </summary>
        public void Saved()
        {
            Dirty = false;
            foreach (ConfigurationVariable cv in Variables)
            {
                cv.Saved();
            }
        }

        /// <summary>
        /// Return a dictionary describing the content of the configuration variable. This dictionary is in use
        /// when saving file to the disk.
        /// </summary>
        /// <returns></returns>
        public abstract object GetDictionary();

        /// <summary>
        /// Is implictly loaded data of current ConfigurationVariable type.
        /// For example "bloop: 5" is implicitly ConfigurationInteger.
        /// </summary>
        /// <param name="fromJson"></param>
        /// <returns></returns>
        public static bool IsImplicitType(JToken fromJson) => throw new NotImplementedException();

        /// <summary>
        /// Is explicitly loaded data of current ConfigurationVariable type.
        /// For example "bloop: { type: int, value: 5}" is explictly ConfigurationInteger.
        /// </summary>
        /// <param name="fromJson"></param>
        /// <returns></returns>
        public static bool IsExplicitType(JToken fromJson) => throw new NotImplementedException();

        /// <summary>
        /// Creates ConfigurationVariable object from loaded data if the data is of the relevant type.
        /// If the data doesnt sute the current type, return null.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromJson"></param>
        /// <param name="father"></param>
        /// <returns></returns>
        public static ConfigurationVariable TryConvert(string name, JToken fromJson, Changable father) => throw new NotImplementedException();

        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                Father.Changed(property);
            }
        }

        /// <summary>
        /// Notify containing object that this variable has changed.
        /// </summary>
        /// <param name="property"></param>
        public void Changed(string property)
        {
            RaisePropertyChanged(property);
            Father.Changed(property);
        }

        public abstract ConfigurationVariable Clone();
        #endregion

    }

    abstract public class ConfigurationVariable<T, G> : ConfigurationVariable where T : InnerType<G>
    {
        /// <summary>
        /// The innert type containing the value read from the variable in the file.
        /// </summary>
        public T Value { get; set; }

        protected ConfigurationVariable(Changable father= null, Brush font_color = null, string name = "", bool is_explicit = false): base(father, font_color, name)
        {
            this.is_explicit = is_explicit;
            IsExplicitnessChangeable = true;
        }

        protected ConfigurationVariable(ConfigurationVariable<T, G> other): base(other)
        {
            UpdateBy(other);
        }

        public override void UpdateBy(ConfigurationVariable other)
        {
            ConfigurationVariable<T, G> o = other as ConfigurationVariable<T, G>;
              base.UpdateBy(o);
            if (Value != null)
            {
                Value.UpdateBy(o.Value);
            } else
            {
                Value = o.Value.Clone() as T;
            }
            

        }

        public static new ConfigurationVariable<T, G> TryConvert(string name, JToken fromJson, Changable father) => throw new NotImplementedException();

        /// <summary>
        /// Updates the value when the view changes. Still needs to be worked on since bounded inner types are not supported.
        /// </summary>
        /// <param name="new_value"></param>
        
        public override object GetDictionary()
        {
            return Value.GetDictionary();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }
}
