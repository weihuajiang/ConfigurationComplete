using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Configuration;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.Windows.Controls.PropertyGrid;

namespace Huarui.Config
{
    /// <summary>
    /// abstract configuration class
    /// </summary>
    [Browsable(false)]
    public class AbstractConfiguration :ConfigurationSection, IConfiguration, ICustomTypeDescriptor
    {
        public AbstractConfiguration()
        {
            GetProperties();
        }
        [Browsable(false)]
        PropertyDescriptorCollection properties = null;
        /// <summary>
        /// disable or enable configuration item
        /// </summary>
        /// <param name="name">configuration name</param>
        /// <param name="enabled"></param>
        public void SetPropertyEnabled(string name, bool enabled)
        {
            if (properties != null)
            {
                for(int i = 0; i < properties.Count; i++)
                {
                    var p = properties[i] as CustomizePropertyDescriptor;
                    if (p.Name.Equals(name))
                    {
                        p.IsEnabled = enabled;
                    }
                }
            }
        }
        /// <summary>
        /// get value for key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected object GetValue(string key)
        {
            return this[key];
        }
        /// <summary>
        /// get value as type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T GetValue<T>(string key, T defaultValue)
        {
            var v = this[key];
            if (v == null)
                return defaultValue;
            return (T)v;
        }
        protected T GetValue<T>(string key)
        {
            var v = this[key];
            if (v == null)
                return default(T);
            return (T)v;
        }
        /// <summary>
        /// change inforamtion
        /// </summary>
        [Browsable(false)]
        public List<ChangeItem> Changes { get; internal set; } = new List<ChangeItem>();
        protected void SetValue(string key, object value)
        {
            object oldV = GetValue(key);
            this[key] = value;
            if ((value != null && !value.Equals(oldV)) || (oldV != null && !oldV.Equals(value)))
            {
                foreach (ChangeItem item in Changes)
                {
                    if (item.Item.Equals(key))
                    {
                        item.NewValue = value;
                        OnPropertyChanged(key);
                        return;
                    }
                }
                Changes.Add(new ChangeItem(key, oldV, value));
                OnPropertyChanged(key);
            }
        }
        protected void SetValue<T>(string key, T value)
        {
            T oldV = GetValue<T>(key);
            this[key] = value;
            if ((value != null && !value.Equals(oldV)) || (oldV != null && !oldV.Equals(value)))
            {
                foreach (ChangeItem item in Changes)
                {
                    if (item.Item.Equals(key))
                    {
                        item.NewValue = value;
                        OnPropertyChanged(key);
                        return;
                    }
                }
                Changes.Add(new ChangeItem(key, oldV, value));
                OnPropertyChanged(key);
            }
        }
        /// <summary>
        /// configuration serialization
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public IConfigurationSerialization Serialization { get; set; } = new ConfigurationSerialization();

        /// <summary>
        /// configuration changed event
        /// </summary>
        [Browsable(false)]
        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Name;
        }
        [Browsable(false)]
        public string Name
        {
            get {
                DisplayNameAttribute dna = GetType().GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault();

                return (dna == null) ? this.GetType().Name : dna.DisplayName;
            }
        }
        
        public virtual void Save()
        {
            Serialization.Save(this);
            Changes.Clear();
        }
        public virtual void Load()
        {
            IConfiguration config = Serialization.Load(this.GetType());
            
            foreach (PropertyInfo pinfo in GetType().GetProperties())
            {
                if (pinfo.GetCustomAttributes(typeof(ConfigurationPropertyAttribute), true).Length > 0)
                {
                    pinfo.SetValue(this, pinfo.GetValue(config, null), null);
                }
            }
            Changes.Clear();
        }
        bool suspendEvent = false;
        protected void OnPropertyChanged(string name)
        {
            if (suspendEvent)
                return;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this,true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return null;
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType,true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes,true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this,true);
        }
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            if (properties == null)
            {
                properties = new PropertyDescriptorCollection(new PropertyDescriptor[] { });
                
                {
                    PropertyDescriptorCollection p;
                    
                        //TypeConverter tc = TypeDescriptor.GetConverter(this);
                        //if (!tc.GetPropertiesSupported())
                            p = TypeDescriptor.GetProperties(this,attributes,true);
                       //else
                        //    p = tc.GetProperties(this);
                    foreach (PropertyDescriptor td in p)
                    {
                        //propToObj.Add(td, inst);
                        if (td.Name.Equals("Name") || td.Name.Equals("Image") || td.Name.Equals("LockItem"))
                            continue;
                        if (!td.IsBrowsable || td.IsReadOnly)
                            continue;
                        properties.Add(new CustomizePropertyDescriptor(td));
                    }
                }
            }
            return properties;
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return GetProperties(new Attribute[] { });
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            if ("Name".Equals(pd.Name) || "Image".Equals(pd.Name))
                return this;
            return this;
        }
    }
}
