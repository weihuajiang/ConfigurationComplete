using Huarui;
using Huarui.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //register service
            Context.AddService("HL7", new FTPLis());
            Context.AddService("ASTM", new ASTMLis());

            //register configuration
            ProcessConfiguration config = new ProcessConfiguration();
            Context.RegisterConfiguration("process", config, GroupVisiblity.ALL, 0);
            DatabaseMaintenanceSetting dConfig = new DatabaseMaintenanceSetting();
            Context.RegisterConfiguration("db", dConfig, GroupVisiblity.ALL, 1);
            Context.RegisterConfiguration("externalQC", GetExternalControlConfigurations(), GroupVisiblity.ALL, 3);
            Context.RegisterConfiguration("lis", new LISConfiguration(), GroupVisiblity.ALL, 2);
            base.OnStartup(e);
        }
        IConfiguration GetExternalControlConfigurations()
        {
            ExternalControlServiceConfiguration configs = new ExternalControlServiceConfiguration();
            configs.Add(new ExternalControlConfiguration1());
            configs.Add(new ExternalControlConfiguration2());
            configs.Add(new ExternalControlConfiguration3());
            return configs;
        }
    }
}
