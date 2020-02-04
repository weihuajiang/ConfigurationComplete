using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Huarui.Config
{
    /// <summary>
    /// configuration serialization to ini file
    /// </summary>
    public class IniFileConfigurationSerialization : IConfigurationSerialization
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpszReturnBuffer, int nSize, string lpFileName);

        public void WriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, FileName);
        }

        public string ReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, FileName);
            if (i <= 0)
                return null;
            return temp.ToString();

        }
        /// <summary>
        /// Ini configuration file name
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="file">Ini configuration file name</param>
        public IniFileConfigurationSerialization(string file)
        {
            FileName = file;
        }
        /// <summary>
        /// Load configuration with Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Load<T>() where T : IConfiguration
        {
            Configuration _cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return (T)Load(typeof(T));
        }
        public IConfiguration Load(Type t)
        {
            IConfiguration config = null;
            if(!File.Exists(FileName))
                return Activator.CreateInstance(t) as IConfiguration;
            string type = t + "";
            config = Activator.CreateInstance(t) as IConfiguration;
            foreach (PropertyInfo pinfo in t.GetProperties())
            {
                if (pinfo.GetCustomAttributes(typeof(ConfigurationPropertyAttribute), true).Length > 0)
                {
                    var name = pinfo.Name;
                    var ctype = pinfo.PropertyType;
                    var value = ReadValue(type, name);
                    if (value == null)
                    {
                        DefaultValueAttribute attr = t.GetCustomAttribute<DefaultValueAttribute>();
                        if (attr != null)
                            pinfo.SetValue(config, attr.Value);
                        else
                        {
                            ConfigurationPropertyAttribute configProperty = t.GetCustomAttribute<ConfigurationPropertyAttribute>();
                            if (configProperty != null && configProperty.DefaultValue!=null)
                            {
                                pinfo.SetValue(config, configProperty.DefaultValue);
                            }
                        }
                        continue;
                    }
                    pinfo.SetValue(config, ConvertTo(value, ctype));
                }
            }
            return config;
        }

        public void Save(IConfiguration config)
        {
            string type = config.GetType() + "";
            Type t = config.GetType();
            foreach (PropertyInfo pinfo in t.GetProperties())
            {
                if (pinfo.GetCustomAttributes(typeof(ConfigurationPropertyAttribute), true).Length > 0)
                {
                    var name = pinfo.Name;
                    var value = CovnertToString(pinfo.GetValue(config));
                    WriteValue(type, name, value);
                }
            }
        }
        private static string CovnertToString(object value)
        {
            if (value != null)
            {
                if (value.GetType().IsEnum)
                {
                    return value.ToString();
                }
            }
            TypeConverter tc = TypeDescriptor.GetConverter(value.GetType());
            return (string)tc.ConvertTo(value,typeof(string));
        }
        private static object ConvertTo(string value, Type t)
        {
            if (t.IsEnum)
            {
                return Enum.Parse(t, value);
            }
            TypeConverter tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value);
        }
    }
}
