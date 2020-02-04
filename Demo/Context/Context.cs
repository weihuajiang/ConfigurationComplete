using Huarui.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huarui
{
    public class Context
    {

        /// <summary>
        /// All service Modules for application
        /// </summary>
        public static Dictionary<string, IService> Services { get; internal set; } = new Dictionary<string, IService>();

        /// <summary>
        /// 运行中增加服务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="serivce"></param>
        public static void AddService(string name, IService serivce)
        {
            Services.Add(name, serivce);
        }
        /// <summary>
        /// 运行中获取特定名称的服务
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IService GetService(string name)
        {
            if (Services.ContainsKey(name))
                return Services[name];
            return null;
        }
        /// <summary>
        /// 运行中获取特定类型的服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : class, IService
        {
            foreach (var t in Services.Values)
            {
                if (t is T)
                    return t as T;
            }
            return null;
        }
        /// <summary>
        /// 运行中获取特定类型的服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetServices<T>() where T : class, IService
        {
            List<T> list = new List<T>();
            foreach (var t in Services.Values)
            {
                if (t is T)
                    list.Add((T)t);
            }
            return list.ToArray();
        }


        class ConfigurationItem
        {
            public ConfigurationItem(string name, IConfiguration config, GroupVisiblity access, int priority)
            {
                Name = name;
                this.config = config;
                this.access = access;
                this.priority = priority;
            }
            public string Name { get; internal set; }
            IConfiguration config;
            public IConfiguration Configuration
            {
                get { return config; }
            }
            GroupVisiblity access;
            public GroupVisiblity Access
            {
                get { return access; }
            }
            int priority;
            public int Priority
            {
                get { return priority; }
            }
        }
        static List<ConfigurationItem> configs = new List<ConfigurationItem>();
        /// <summary>
        /// Register configuration
        /// </summary>
        /// <param name="config"></param>
        /// <param name="access"></param>
        /// <param name="priority"></param>
        public static void RegisterConfiguration(string name, IConfiguration config, GroupVisiblity access, int priority)
        {
            int index = configs.Count;
            for (int i = 0; i < configs.Count; i++)
            {
                if (priority < configs[i].Priority)
                {
                    index = i;
                    break;
                }
            }
            configs.Insert(index, new ConfigurationItem(name, config, access, priority));
            config.Load();
        }
        static ConfigurationCollection _configurations = new ConfigurationCollection();
        private static ConfigurationCollection GetConfigurations(UserGroup access)
        {
            _configurations.Clear();
            foreach (ConfigurationItem item in configs)
            {
                GroupVisiblity role = item.Access;
                if (!role.IsVisible(access))
                    continue;
                IConfiguration c = item.Configuration;
                if (c == null)
                    continue;
                _configurations.Add(c);
            }
            return _configurations;
        }
        /// <summary>
        /// Configuration object for current user to display in configuration editor
        /// </summary>
        public static ConfigurationCollection Configurations
        {
            get
            {
                //int group = 0;
                //if (UserManager != null && UserManager.CurrentUser != null)
                //    group = (int)UserManager.CurrentUser.Group;
                return GetConfigurations(UserGroup.Service);// _configurations;
            }
        }
        public static T GetConfiguration<T>() where T : class, IConfiguration
        {
            foreach (var t in configs)
            {
                if (t.Configuration is T)
                    return t.Configuration as T;
            }
            foreach (var t in configs)
            {
                if (t.Configuration is ConfigurationCollection)
                {
                    var c = t.Configuration as ConfigurationCollection;
                    var e = GetConfigurationIn<T>(c);
                    if (e != null)
                        return e;
                }
            }
            return null;
        }
        static T GetConfigurationIn<T>(ConfigurationCollection c) where T : class, IConfiguration
        {
            foreach (var cs in c)
            {
                if (cs is T)
                    return cs as T;
                if (cs is ConfigurationCollection)
                {
                    var t = GetConfigurationIn<T>(cs as ConfigurationCollection);
                    if (t != null)
                        return t;
                }
            }
            return null;
        }
    }
}
