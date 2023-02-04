using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Windows.Controls.PropertyGrid
{
    public class CustomizePropertyDescriptor : PropertyDescriptor
    {
        PropertyDescriptor descriptor;
        public CustomizePropertyDescriptor(PropertyDescriptor d, bool isEnabled=true):base(d)
        {
            descriptor = d;
            IsEnabled = isEnabled;
        }
        public event EventHandler IsEnabledChanged;
        public bool enabled = true;
        public bool IsEnabled {
            get
            {
                return enabled;
            }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    if (IsEnabledChanged != null)
                    {
                        IsEnabledChanged(this, new EventArgs());
                    }
                }
            }
        }
        public override string Category => descriptor.Category;
        public override TypeConverter Converter => descriptor.Converter;
        public override string Description => descriptor.Description;
        public override string DisplayName => descriptor.DisplayName;
        public override bool IsBrowsable => descriptor.IsBrowsable;
        public override bool IsLocalizable => descriptor.IsLocalizable;
        public override Type ComponentType => descriptor.ComponentType;
        public override string Name => descriptor.Name;
        public override bool SupportsChangeEvents => descriptor.SupportsChangeEvents;
        public override bool IsReadOnly => descriptor.IsReadOnly;

        public override Type PropertyType => descriptor.PropertyType;

        public override bool CanResetValue(object component)
        {
            return descriptor.CanResetValue(component);
        }

        public override object GetValue(object component)
        {
            return descriptor.GetValue(component);
        }

        public override void ResetValue(object component)
        {
            descriptor.ResetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            descriptor.SetValue(component, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return descriptor.ShouldSerializeValue(component);
        }
    }
}
