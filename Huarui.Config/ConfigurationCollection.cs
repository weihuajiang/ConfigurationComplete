using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using Microsoft.Windows.Controls.PropertyGrid;

namespace Huarui.Config
{
    /// <summary>
    /// configuration collection
    /// </summary>
    public class ConfigurationCollection : ObservableCollection<IConfiguration>, ICustomTypeDescriptor,
        IConfiguration
    {
        [Browsable(false)]
        public virtual IConfigurationSerialization Serialization { get; set; } = new ConfigurationSerialization();

        public event PropertyChangedEventHandler ConfigurationPropertyChanged;

        protected override void InsertItem(int index, IConfiguration item)
        {
            base.InsertItem(index, item);
            item.PropertyChanged += item_PropertyChanged;
        }
        protected override void SetItem(int index, IConfiguration item)
        {
            this[index].PropertyChanged -= item_PropertyChanged;
            base.SetItem(index, item);
                item.PropertyChanged += item_PropertyChanged;
        }
        protected override void ClearItems()
        {
            for (int i = 0; i < this.Count; i++)
                this[i].PropertyChanged -= item_PropertyChanged;
            base.ClearItems();
        }
        protected override void RemoveItem(int index)
        {
            IConfiguration item = this[index];
            item.PropertyChanged -= item_PropertyChanged;
            base.RemoveItem(index);
        }
        void item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("IsChanged"))
            {
                changedConfig.Add(sender as IConfiguration);
                changedProperty.Add(e.PropertyName);
                PropertyChangedEventHandler handler = ConfigurationPropertyChanged;
                if (handler != null)
                    handler(sender, e);
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
                //if (changedConfig.Count == 1)
                //bool a = IsChanged;
                OnPropertyChanged(new PropertyChangedEventArgs("IsChanged"));
            }
        }

        public bool IsChanged
        {
            get { return changedConfig.Count > 0; }
        }
        [Browsable(false)]
        List<IConfiguration> changedConfig = new List<IConfiguration>();
        [Browsable(false)]
        List<string> changedProperty = new List<string>();

        [Browsable(false)]
        //public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Name;
        }
        [Browsable(false)]
        public string Name
        {
            get
            {
                DisplayNameAttribute dna = GetType().GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault();
                return (dna == null) ? this.GetType().Name : dna.DisplayName;
            }
        }
        public virtual void Save()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].Save();
            }
            changedConfig.Clear();
            changedProperty.Clear();
            OnPropertyChanged(new PropertyChangedEventArgs("IsChanged"));
        }
        public virtual void Load()
        {
            for (int i = 0; i < Count; i++)
                this[i].Load();
            changedConfig.Clear();
            changedProperty.Clear();
            OnPropertyChanged(new PropertyChangedEventArgs("IsChanged"));
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this);
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
            return TypeDescriptor.GetEditor(this, editorBaseType);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this);
        }
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
                PropertyDescriptorCollection properties = new PropertyDescriptorCollection(new PropertyDescriptor[] { });
                foreach (object inst in this)
                {
                    PropertyDescriptorCollection p;
                    if (inst is ICustomTypeDescriptor)
                        p = ((ICustomTypeDescriptor)inst).GetProperties(attributes);
                    else
                    {
                        TypeConverter tc = TypeDescriptor.GetConverter(inst);
                        if (!tc.GetPropertiesSupported())
                            p = TypeDescriptor.GetProperties(inst);
                        else
                            p = tc.GetProperties(inst);
                    }
                    foreach (PropertyDescriptor td in p)
                    {
                        if (td.Name.Equals("Name") || td.Name.Equals("Image") || td.Name.Equals("LockItem"))
                            continue;
                        if (!td.IsBrowsable || td.IsReadOnly)
                            continue;
                        if (td is CustomizePropertyDescriptor)
                            properties.Add(td);
                        else
                            properties.Add(new CustomizePropertyDescriptor(td));
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
            foreach (object inst in this)
            {
                if (pd.ComponentType == inst.GetType())
                {
                    return inst;
                }
            }
            return this;
        }
    }
}
