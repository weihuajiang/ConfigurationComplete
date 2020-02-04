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
    [DisplayName("LIS")]
    class LISConfiguration : ConfigurationCollection
    {
        LISServiceConfiguration lisConfig = new LISServiceConfiguration();
        public LISConfiguration()
        {
            lisConfig.Serialization = new ConfigurationSerialization();
            Add(lisConfig);
            lisConfig.PropertyChanged += Config_PropertyChanged;
        }

        IConfiguration _config;
        [Browsable(false)]
        public IConfiguration HostInterfaceConfig
        {
            get
            {
                if (_config != null)
                    return _config;
                return null;
            }
            set
            {
                if (_config != value)
                {
                    if (_config != null && this.Contains(_config))
                        this.Remove(_config);
                    _config = value;
                    if (value != null)
                    {
                        Add(_config);
                    }
                }
            }
        }
        private void Config_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ServiceType"))
            {
                if (lisConfig.IsLISServiceEnabled)
                {
                    bool find = false;
                    foreach (ILisService ihs in Context.GetServices<ILisService>())
                        if (lisConfig.ServiceType.StartsWith(ihs.GetType().FullName + ", "))
                        {
                            if (ihs.Configuration == null)
                            {
                                HostInterfaceConfig = null;
                                return;
                            }
                            if (!ihs.Configuration.Equals(HostInterfaceConfig))
                            {
                                ihs.Configuration.Serialization = new ConfigurationSerialization();
                                ihs.Configuration.Load();
                                HostInterfaceConfig = ihs.Configuration;
                                //HostInterfaceConfig.Load();
                            }
                            find = true;
                        }
                    if (!find)
                    {
                        HostInterfaceConfig = null;
                    }

                }
                else
                {
                    HostInterfaceConfig = null;
                }
            }
            else if (e.PropertyName.Equals("IsLISServiceEnabled"))
            {
                if (lisConfig.IsLISServiceEnabled)
                {
                    foreach (ILisService ihs in Context.GetServices<ILisService>())
                    {
                        if (lisConfig.ServiceType.StartsWith(ihs.GetType().FullName + ", "))
                        {
                            if (ihs.Configuration == null)
                            {
                                HostInterfaceConfig = null;
                            }
                            else
                            {
                                ihs.Configuration.Serialization = new ConfigurationSerialization();
                                ihs.Configuration.Load();
                                HostInterfaceConfig = ihs.Configuration;
                            }
                        }
                    }
                }
                else
                {
                    HostInterfaceConfig = null;
                }
            }
        }
    }
}
