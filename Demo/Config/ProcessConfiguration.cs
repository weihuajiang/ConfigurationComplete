using Huarui.Config;
using Microsoft.Windows.Controls.PropertyGrid.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Huarui
{
    [DisplayName("Test Workflow")]
    public class ProcessConfiguration : AbstractConfiguration
    {
        [DispId(0)]
        [Category("Test Workflow")]
        [DisplayName("Pool Type")]
        [Description("Pool Type")]
        [ConfigurationProperty("PoolSize")]
        [DefaultValue(PoolSize.Pool8)]
        public PoolSize PoolSize
        {
            get
            {
                return GetValue<PoolSize>("PoolSize");
            }
            set
            {
                SetValue("PoolSize", value);
                SetPropertyEnabled("IsPool8Enabled", value == PoolSize.Pool8);
            }
        }
        [DispId(4)]
        [Category("Test Workflow")]
        [DisplayName("Enable 8 Pool Workflow")]
        [Description("是否允许8样品汇集检测体会样品")]
        [DefaultValue(false)]
        [ConfigurationProperty("IsPool8Enabled")]
        public bool IsPool8Enabled
        {
            get
            {
                return GetValue<bool>("IsPool8Enabled");
            }
            set
            {
                SetValue("IsPool8Enabled", value);
            }
        }
        [DispId(3)]
        [Category("Test Workflow")]
        [DisplayName("Sample Release Days")]
        [Description("如果样品检测一直没有结束，经过指定天数后，该样品会被释放，从而可以使用该条码重新开始检测")]
        [ConfigurationProperty("SampleReleaseDays")]
        [DefaultValue(14)]
        public int SampleReleaseDays
        {
            get
            {
                return GetValue<int>("SampleReleaseDays");
            }
            set
            {
                SetValue<int>("SampleReleaseDays", value);
            }
        }
    }
    public enum PoolSize
    {
        [EnumDisplayName("8 Pool")]
        Pool8,
        [EnumDisplayName("48 Pool")]
        Pool48
    }
}
