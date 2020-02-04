using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace Huarui.Config
{
    /// <summary>
    /// Configuration Serialization to exe configuration file
    /// </summary>
    public class ConfigurationSerialization : IConfigurationSerialization
    {
        public void Save(IConfiguration config)
        {
            Configuration _cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Save(_cfg, config);
            _cfg.Save(ConfigurationSaveMode.Minimal);
        }
        private static void Save(Configuration configFil, IConfiguration config)
        {
            if (config is ConfigurationCollection)
            {
                ConfigurationCollection collection = config as ConfigurationCollection;
                foreach (IConfiguration c in collection)
                {
                    if (c is ConfigurationCollection)
                    {
                        Save(configFil, c);
                    }
                    else if (c is ConfigurationSection)
                    {
                        
                        ConfigurationSection _section = configFil.Sections[c.GetType().Name];
                        if (_section != null)
                        {
                            foreach (PropertyInfo pinfo in c.GetType().GetProperties())
                            {
                                if (pinfo.GetCustomAttributes(typeof(ConfigurationPropertyAttribute), true).Length > 0)
                                {
                                    pinfo.SetValue(_section, pinfo.GetValue(c, null), null);
                                }
                            }
                        }
                        else
                        {
                            configFil.Sections.Add(c.GetType().Name, c as ConfigurationSection);
                        }
                    }
                }

            }
            else
            {
                if (config is ConfigurationSection)
                {
                    ConfigurationSection _section = configFil.Sections[config.GetType().Name];
                    if (_section != null)
                    {
                        foreach (PropertyInfo pinfo in config.GetType().GetProperties())
                        {
                            if (pinfo.GetCustomAttributes(typeof(ConfigurationPropertyAttribute), true).Length > 0)
                            {
                                pinfo.SetValue(_section, pinfo.GetValue(config, null), null);
                            }
                        }
                    }
                    else
                    {
                        configFil.Sections.Add(config.GetType().Name, config as ConfigurationSection);
                    }
                }
            }
        }
        public IConfiguration Load(Type t)
        {

            Configuration _cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return Load(_cfg, t);
        }
        public T Load<T>() where T : IConfiguration
        {

            Configuration _cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return (T)Load(_cfg, typeof(T));
        }
        private static IConfiguration Load(Configuration configFil, Type t)
        {
            IConfiguration config = Activator.CreateInstance(t) as IConfiguration;
            if (config is ConfigurationCollection)
            {
                ConfigurationCollection collection = config as ConfigurationCollection;
                for (int i = 0; i < collection.Count; i++)
                {
                    collection[i] = Load(configFil, collection[i].GetType());
                }
            }
            else
            {
                IConfiguration _section = configFil.Sections[t.Name] as IConfiguration;
                if (_section != null)
                    config = _section as IConfiguration;
            }
            return config;
        }
    }
}
