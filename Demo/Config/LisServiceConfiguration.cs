using Huarui.Config;
using Microsoft.Windows.Controls.PropertyGrid.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Huarui
{
    class LISTypeEditor : ComboBoxEditor, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                foreach (var ihs in Context.GetServices<ILisService>())
                {
                    var t = ihs.GetType();
                    string name = t.FullName + ", " + t.Assembly.FullName.Substring(0, t.Assembly.FullName.IndexOf(","));
                    if (name.Equals(value + ""))
                        return ihs.Type;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                foreach (var ihs in Context.GetServices<ILisService>())
                {
                    var t = ihs.GetType();
                    string name = t.FullName + ", " + t.Assembly.FullName.Substring(0, t.Assembly.FullName.IndexOf(","));
                    if (ihs.Type.Equals(value + ""))
                        return name;

                }
            }
            return value;
        }

        protected override IList<object> CreateItemsSource(Microsoft.Windows.Controls.PropertyGrid.PropertyItem propertyItem)
        {
            List<object> values = new List<object>();
            values.Add("");
            foreach (var ihs in Context.GetServices<ILisService>())
            {
                var t = ihs.GetType();
                string name = t.FullName + ", " + t.Assembly.FullName.Substring(0, t.Assembly.FullName.IndexOf(","));
                values.Add(ihs.Type);
            }
            return values;
        }
        protected override IValueConverter CreateValueConverter()
        {
            return this;
        }
    }
    [DisplayName("General")]
    public class LISServiceConfiguration : AbstractConfiguration
    {
        [Category("LIS Server Setting")]
        [DisplayName("LIS Servier enabled")]
        [Description("是否使用LIS服务器")]
        [DefaultValue(false)]
        [ConfigurationProperty("IsLISServiceEnabled")]
        [DispId(1)]
        public bool IsLISServiceEnabled
        {
            get
            {
                if (this["IsLISServiceEnabled"] == null)
                    return false;
                return (bool)this["IsLISServiceEnabled"];
            }
            set
            {
                SetValue("IsLISServiceEnabled", value);
                SetPropertyEnabled("UploadAcceptedResult", value);
                SetPropertyEnabled("IsAutomaticUpload", value);
                SetPropertyEnabled("ServiceType", value);
            }
        }

        [Category("LIS Server Setting")]
        [DisplayName("Communication Type")]
        [Description("LIS服务器连接类型")]
        [DefaultValue("")]
        [ConfigurationProperty("ServiceType")]
        [Editor(typeof(LISTypeEditor), typeof(LISTypeEditor))]
        [DispId(2)]
        public string ServiceType
        {
            get
            {
                return (string)this["ServiceType"];
            }
            set
            {
                SetValue("ServiceType", value);
            }
        }

        [Category("Data upload setting")]
        [DisplayName("Only upload accepted data")]
        [Description("只有经过用户确认后的结果，才允许上传到LIS服务器上")]
        [DefaultValue(true)]
        [ConfigurationProperty("UploadAcceptedResult")]
        [DispId(3)]
        public bool UploadAcceptedResult
        {
            get
            {
                if (this["UploadAcceptedResult"] == null)
                    return true;
                return (bool)this["UploadAcceptedResult"];
            }
            set
            {
                SetValue("UploadAcceptedResult", value);
            }
        }
        [Category("Data upload setting")]
        [DisplayName("Automatic Upload result to LIS")]
        [Description("检测结果完成或者用户接受结果后，自动上传检测结果")]
        [DefaultValue(false)]
        [ConfigurationProperty("IsAutomaticUpload")]
        [DispId(4)]
        public bool IsAutomaticUpload
        {
            get
            {
                if (this["IsAutomaticUpload"] == null)
                    return true;
                return (bool)this["IsAutomaticUpload"];
            }
            set
            {
                SetValue("IsAutomaticUpload", value);
            }
        }
    }
}
