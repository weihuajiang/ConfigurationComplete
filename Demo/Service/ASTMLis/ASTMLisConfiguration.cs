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

namespace Huarui
{
    [DisplayName("ASTM LIS")]
    public class ASTMLisConfiguration : AbstractConfiguration
    {
        [Category("ASTM LIS Setting")]
        [DisplayName("Address")]
        [Description("连接的文件共享服务器地址，支持本地目录、其他电脑共享目录和FTP")]
        [ConfigurationProperty("Address")]
        [DefaultValue(@"\\192.168.0.200\SharedFolder")]
        public string Address
        {
            get
            {
                if (this["Address"] == null)
                    return @"\\192.168.0.200\SharedFolder";
                return (string)this["Address"];
            }
            set
            {
                SetValue("Address", value);
            }
        }
        [Category("ASTM LIS Setting")]
        [DisplayName("File Name Prefix")]
        [Description("结果文件名称前缀")]
        [ConfigurationProperty("FileNamePrefix")]
        [DefaultValue("Result")]
        public string FileNamePrefix
        {
            get
            {
                if (this["FileNamePrefix"] == null)
                    return "Result";
                return (string)this["FileNamePrefix"];
            }
            set
            {
                SetValue("FileNamePrefix", value);
            }
        }
    }
}
