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
    [DisplayName("Database")]
    public class DatabaseMaintenanceSetting : AbstractConfiguration
    {
        public DatabaseMaintenanceSetting()
        {
        }
        [DispId(0)]
        [Category("Backup Setting")]
        [DisplayName("Enable Automatic Backup")]
        [Description("是否自动备份数据库")]
        [ConfigurationProperty("IsBackupEnabled")]
        [DefaultValue(false)]
        public bool IsBackupEnabled
        {
            get
            {
                if (GetValue("IsBackupEnabled") == null)
                    return false;
                return (bool)GetValue("IsBackupEnabled");
            }
            set
            {
                SetValue("IsBackupEnabled", value);
                if (value)
                {
                    SetPropertyEnabled("BackupInterval", true);
                }
                else
                    SetPropertyEnabled("BackupInterval", false);
            }
        }
        [DispId(1)]
        [Category("Backup Setting")]
        [DisplayName("Automatic Backup Interval (Days)")]
        [Description("间隔多少天备份一次数据库")]
        [ConfigurationProperty("BackupInterval")]
        [DefaultValue(1)]
        public int BackupInterval
        {
            get
            {
                if (GetValue("BackupInterval") == null)
                    return 1;
                return (int)GetValue("BackupInterval");
            }
            set
            {
                SetValue("BackupInterval", value);
            }
        }
        [DispId(2)]
        [Category("Backup Setting")]
        [DisplayName("Backup Destination")]
        [Description("数据库备份文件保存位置，目前支持本地目录，其他电脑共享目录和FTP")]
        [ConfigurationProperty("BackupDestination")]
        [DefaultValue("D:\\Backup")]
        public string BackupDestination
        {
            get
            {
                if (GetValue("BackupDestination") == null)
                    return "D:\\Backup";
                return (string)GetValue("BackupDestination");
            }
            set
            {
                SetValue("BackupDestination", value);
                if (!string.IsNullOrEmpty(value) && (value.StartsWith(@"\\") || value.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase)))
                {
                    SetPropertyEnabled("BackupUser", true);
                    SetPropertyEnabled("BackupPassword", true);
                }
                else
                {
                    SetPropertyEnabled("BackupUser", false);
                    SetPropertyEnabled("BackupPassword", false);
                }
            }
        }
        [DispId(3)]
        [Category("Backup Setting")]
        [DisplayName("Access account for Backup Destination")]
        [Description("数据库备份文件保存位置访问时候使用的用户名")]
        [ConfigurationProperty("BackupUser")]
        [DefaultValue("")]
        public string BackupUser
        {
            get
            {
                if (GetValue("BackupUser") == null)
                    return "";
                return (string)GetValue("BackupUser");
            }
            set
            {
                SetValue("BackupUser", value);
            }
        }
        [DispId(4)]
        [Category("Backup Setting")]
        [DisplayName("Access password for Backup Destination")]
        [Description("数据库备份文件保存位置访问时候使用的密码")]
        [ConfigurationProperty("BackupPassword")]
        [DefaultValue("")]
        [Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
        public string BackupPassword
        {
            get
            {
                if (GetValue("BackupPassword") == null)
                    return "";
                return (string)GetValue("BackupPassword");
            }
            set
            {
                SetValue("BackupPassword", value);
            }
        }
        [DispId(5)]
        [Category("Backup Setting")]
        [DisplayName("Keep backup data file number")]
        [Description("保留备份文件数目，备份文件超过这个数目后，将会将旧的备份文件删除")]
        [ConfigurationProperty("BackupCount")]
        [DefaultValue(5)]
        public int BackupCount
        {
            get
            {
                if (GetValue("BackupCount") == null)
                    return 5;
                return (int)GetValue("BackupCount");
            }
            set
            {
                SetValue("BackupCount", value);
            }
        }
        [DispId(6)]
        [Category("Archive Setting")]
        [DisplayName("Enable Automatic Archive")]
        [Description("是否对数据库中很久的数据进行归档操作，并清除归档的数据")]
        [ConfigurationProperty("IsArchiveEnabled")]
        [DefaultValue(false)]
        public bool IsArchiveEnabled
        {
            get
            {
                if (GetValue("IsArchiveEnabled") == null)
                    return false;
                return (bool)GetValue("IsArchiveEnabled");
            }
            set
            {
                SetValue("IsArchiveEnabled", value);
                if (value)
                {
                    SetPropertyEnabled("ArchiveInterval", true);
                }
                else
                    SetPropertyEnabled("ArchiveInterval", false);
            }
        }
        [DispId(7)]
        [Category("Archive Setting")]
        [DisplayName("Archive Destination")]
        [Description("数据库归档后，结果保存位置，目前支持本地目录，其他电脑共享目录和FTP")]
        [DefaultValue("D:\\Archive")]
        [ConfigurationProperty("ArchivePath")]
        public string ArchivePath
        {
            get
            {
                if (GetValue("ArchivePath") == null)
                    return "D:\\Archive";
                return (string)GetValue("ArchivePath");
            }
            set
            {
                SetValue("ArchivePath", value);
                if (!string.IsNullOrEmpty(value) && (value.StartsWith(@"\\") || value.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase)))
                {
                    SetPropertyEnabled("ArchiveUser", true);
                    SetPropertyEnabled("ArchivePassword", true);
                }
                else
                {
                    SetPropertyEnabled("ArchiveUser", false);
                    SetPropertyEnabled("ArchivePassword", false);
                }
            }
        }
        [DispId(8)]
        [Category("Archive Setting")]
        [DisplayName("Access acount for Archive destination")]
        [Description("数据库归档结果保存位置访问用户名")]
        [DefaultValue("")]
        [ConfigurationProperty("ArchiveUser")]
        public string ArchiveUser
        {
            get
            {
                if (GetValue("ArchiveUser") == null)
                    return "";
                return (string)GetValue("ArchiveUser");
            }
            set
            {
                SetValue("ArchiveUser", value);
            }
        }
        [DispId(9)]
        [Category("Archive Setting")]
        [DisplayName("Access password for Archive destination")]
        [Description("数据库归档结果保存位置访问密码")]
        [DefaultValue("")]
        [ConfigurationProperty("ArchivePassword")]
        [Editor(typeof(PasswordEditor), typeof(PasswordEditor))]
        public string ArchivePassword
        {
            get
            {
                if (GetValue("ArchivePassword") == null)
                    return "";
                return (string)GetValue("ArchivePassword");
            }
            set
            {
                SetValue("ArchivePassword", value);
            }
        }
        [DispId(10)]
        [Category("Archive Setting")]
        [DisplayName("Archive data older than (days)")]
        [Description("结果在数据库超过指定时间，将被归档")]
        [ConfigurationProperty("ArchiveResultOlderThan")]
        [DefaultValue(180)]
        public int ArchiveResultOlderThan
        {
            get
            {
                if (this["ArchiveResultOlderThan"] == null)
                    return 180;
                return (int)this["ArchiveResultOlderThan"];
            }
            set
            {
                SetValue("ArchiveResultOlderThan", value);
            }
        }
        [DispId(11)]
        [Category("Archive Setting")]
        [DisplayName("Automatic Archive Scheduler Interval (days)")]
        [Description("进行数据库归档操作的间隔时间")]
        [ConfigurationProperty("ArchiveInterval")]
        [DefaultValue(90)]
        public int ArchiveInterval
        {
            get
            {
                if (GetValue("ArchiveInterval") == null)
                    return 90;
                return (int)GetValue("ArchiveInterval");
            }
            set
            {
                SetValue("ArchiveInterval", value);
            }
        }


        [DispId(12)]
        [Category("Purge Setting")]
        [DisplayName("Reagent and Consumable information keep days")]
        [Description("试剂和耗材信息超过天数后，会自动清楚")]
        [ConfigurationProperty("CountKeepDays")]
        [DefaultValue(180)]
        public int CountKeepDays
        {
            get
            {
                if (GetValue("CountKeepDays") == null)
                    return 180;
                return (int)GetValue("CountKeepDays");
            }
            set
            {
                SetValue("CountKeepDays", value);
            }
        }
        [DispId(13)]
        [Category("Purge Setting")]
        [DisplayName("Alarm keep days")]
        [Description("报警信息超过天数后，会自动清楚")]
        [ConfigurationProperty("MessageKeepDays")]
        [DefaultValue(180)]
        public int MessageKeepDays
        {
            get
            {
                if (GetValue("MessageKeepDays") == null)
                    return 60;
                return (int)GetValue("MessageKeepDays");
            }
            set
            {
                SetValue("MessageKeepDays", value);
            }
        }
        [DispId(14)]
        [Category("Purge Setting")]
        [DisplayName("Audit trial keep days")]
        [Description("审计信息超过天数后，会自动清楚")]
        [ConfigurationProperty("AuditKeepDays")]
        [DefaultValue(90)]
        public int AuditKeepDays
        {
            get
            {
                if (GetValue("AuditKeepDays") == null)
                    return 60;
                return (int)GetValue("AuditKeepDays");
            }
            set
            {
                SetValue("AuditKeepDays", value);
            }
        }
        [DispId(15)]
        [Category("Purge Setting")]
        [DisplayName("Log files keep days")]
        [Description("日志文件超过天数后，会自动清楚")]
        [ConfigurationProperty("LogFileKeepDays")]
        [DefaultValue(90)]
        public int LogFileKeepDays
        {
            get
            {
                if (GetValue("LogFileKeepDays") == null)
                    return 60;
                return (int)GetValue("LogFileKeepDays");
            }
            set
            {
                SetValue("LogFileKeepDays", value);
            }
        }
    }
}
