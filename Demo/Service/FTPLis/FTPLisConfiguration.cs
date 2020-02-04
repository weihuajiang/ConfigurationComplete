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
    public enum ExportFileFormat
    {
        Excel,
        Xml
    }
    [DisplayName("FTP")]
    public class FTPLisConfiguration : AbstractConfiguration
    {
        [Category("FTP LIS Setting")]
        [DisplayName("Server Address")]
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
                var v = (!string.IsNullOrEmpty(value) && (value.StartsWith(@"\\") || value.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase)));
                    SetPropertyEnabled("User", v);
                SetPropertyEnabled("Password", v);
            }
        }
        [Category("FTP LIS Setting")]
        [DisplayName("Access User")]
        [Description("登录服务器的用户名")]
        [ConfigurationProperty("User")]
        public string User
        {
            get
            {
                if (this["User"] == null)
                    return "";
                return (string)this["User"];
            }
            set
            {
                SetValue("User", value);
            }
        }
        [Category("FTP LIS Setting")]
        [DisplayName("Access Password")]
        [Description("登录服务器的密码")]
        [ConfigurationProperty("Password")]
        [Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
        public string Password
        {
            get
            {
                if (this["Password"] == null)
                    return "";
                return (string)this["Password"];
            }
            set
            {
                SetValue("Password", value);
            }
        }
        [Category("FTP LIS Setting")]
        [DisplayName("File Format")]
        [Description("结果文件格式，目前支持Excel和Xml两种格式")]
        [ConfigurationProperty("ExportFileFormat")]
        [DefaultValue(ExportFileFormat.Excel)]
        public ExportFileFormat ExportFileFormat
        {
            get
            {
                if (this["ExportFileFormat"] == null)
                    return ExportFileFormat.Excel;
                return (ExportFileFormat)this["ExportFileFormat"];
            }
            set
            {
                SetValue("ExportFileFormat", value);
            }
        }
        [Category("FTP LIS Setting")]
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
