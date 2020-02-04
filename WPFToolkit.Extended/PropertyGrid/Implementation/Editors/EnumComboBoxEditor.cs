using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Data;

namespace Microsoft.Windows.Controls.PropertyGrid.Editors
{

    [AttributeUsage(AttributeTargets.All)]
    public class EnumDisplayNameAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public EnumDisplayNameAttribute(string display)
        {
            DisplayName = display;
        }
    }
    /// <summary>
    /// EnumEditor for enum with EnumDisplayNameAttribute
    /// </summary>
    public class EnumEditor : ComboBoxEditor
    {
        Type _type;
        protected override IList<object> CreateItemsSource(PropertyItem propertyItem)
        {
            _type = propertyItem.PropertyType;
            return GetValues(propertyItem.PropertyType);
        }

        private static object[] GetValues(Type enumType)
        {
            List<object> values = new List<object>();

            var fields = enumType.GetFields().Where(x => x.IsLiteral);
            foreach (FieldInfo field in fields)
            {
                // Get array of BrowsableAttribute attributes
                object[] attrs = field.GetCustomAttributes(typeof(BrowsableAttribute), false);
                if (attrs.Length == 1)
                {
                    // If attribute exists and its value is false continue to the next field...
                    BrowsableAttribute brAttr = (BrowsableAttribute)attrs[0];
                    if (brAttr.Browsable == false) continue;
                }
                object[] att2 = field.GetCustomAttributes(typeof(EnumDisplayNameAttribute), false);
                if (att2.Length > 0)
                {
                    values.Add((att2[0] as EnumDisplayNameAttribute).DisplayName);
                }
                else
                    values.Add(field.GetValue(enumType));
            }

            return values.ToArray();
        }
        protected override IValueConverter CreateValueConverter()
        {
            return new EnumValueConverter(_type);
        }
    }
    public class EnumValueConverter : IValueConverter
    {
        Type _type;
        public EnumValueConverter(Type enumType)
        {
            _type = enumType;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FieldInfo[] fields = _type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].IsLiteral)
                {
                    object fvalue = fields[i].GetValue(_type);
                    if (fvalue.Equals(value))
                    {
                        object[] att2 = fields[i].GetCustomAttributes(typeof(EnumDisplayNameAttribute), false);
                        if (att2.Length > 0)
                        {
                            return (att2[0] as EnumDisplayNameAttribute).DisplayName;
                        }
                    }
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FieldInfo[] fields = _type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].IsLiteral)
                {
                    object fvalue = fields[i].GetValue(_type);
                    if (fvalue.Equals(value))
                        return fvalue;
                    object[] att2 = fields[i].GetCustomAttributes(typeof(EnumDisplayNameAttribute), true);
                    if (att2.Length > 0)
                    {
                        if ((att2[0] as EnumDisplayNameAttribute).DisplayName.Equals(value))
                        {
                            return fvalue;
                        }
                    }
                }
            }
            return value;
        }
    }
}
