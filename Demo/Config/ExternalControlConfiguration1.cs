using Huarui.Config;
using Microsoft.Windows.Controls.PropertyGrid.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Huarui
{/// <summary>
 /// 外部质控1
 /// </summary>
    [DisplayName("External Control-1")]
    public class ExternalControlConfiguration1 : AbstractConfiguration
    {
        public ExternalControlConfiguration1()
        {
            Serialization = new IniFileConfigurationSerialization(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "external1.ini"));
        }
        [Category("External Control")]
        [DisplayName("Enable External Control-1")]
        [Description("Enable External Control-1")]
        [ConfigurationProperty("IsControlEnabled")]
        public bool IsControlEnabled
        {
            get
            {
                return (bool)GetValue("IsControlEnabled");
            }
            set
            {
                SetValue("IsControlEnabled", value);
                SetPropertyEnabled("ControlName", value);
                SetPropertyEnabled("BarcodeMarsk", value);
                SetPropertyEnabled("Volume", value);
                SetPropertyEnabled("HBVResult", value);
                SetPropertyEnabled("HCVResult", value);
                SetPropertyEnabled("HIVResult", value);
            }
        }
        [Category("External Control-1")]
        [DisplayName("Name")]
        [Description("Exernal Control Name")]
        [ConfigurationProperty("ControlName")]
        [DispId(2)]
        public string ControlName
        {
            get
            {
                return (string)GetValue("ControlName");
            }
            set
            {
                SetValue("ControlName", value);
            }
        }
        [Category("External Control-1")]
        [DisplayName("Barcode Mask")]
        [Description("Control Barcode Mask")]
        [ConfigurationProperty("BarcodeMarsk")]
        [DispId(3)]
        public string BarcodeMarsk
        {
            get
            {
                return (string)GetValue("BarcodeMarsk");
            }
            set
            {
                SetValue("BarcodeMarsk", value);
            }
        }
        [Category("External Control-1")]
        [DisplayName("Volume")]
        [Description("Control Used Volume per assay")]
        [ConfigurationProperty("Volume")]
        [DispId(4)]
        public double Volume
        {
            get
            {
                return (double)GetValue("Volume");
            }
            set
            {
                SetValue("Volume", value);
            }
        }
        [Category("External Control-1")]
        [DisplayName("HBV Result")]
        [Description("HBV Channel Result expected")]
        [ConfigurationProperty("HBVResult")]
        [DispId(5)]
        public ExpectResultEnum HBVResult
        {
            get
            {
                return (ExpectResultEnum)GetValue("HBVResult");
            }
            set
            {
                SetValue("HBVResult", value);
            }
        }
        [Category("External Control-1")]
        [DisplayName("HCV Result")]
        [Description("HCV Channel Result expected")]
        [ConfigurationProperty("HCVResult")]
        [DispId(6)]
        public ExpectResultEnum HCVResult
        {
            get
            {
                return (ExpectResultEnum)GetValue("HCVResult");
            }
            set
            {
                SetValue("HCVResult", value);
            }
        }
        [Category("External Control-1")]
        [DisplayName("HIV Result")]
        [Description("HIV Channel Result expected")]
        [ConfigurationProperty("HIVResult")]
        [DispId(7)]
        public ExpectResultEnum HIVResult
        {
            get
            {
                return (ExpectResultEnum)GetValue("HIVResult");
            }
            set
            {
                SetValue("HIVResult", value);
            }
        }
    }
}
