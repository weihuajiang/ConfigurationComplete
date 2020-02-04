using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huarui.Config
{
    /// <summary>
    /// Configuration interface
    /// </summary>
    public interface IConfiguration : INotifyPropertyChanged
    {
        IConfigurationSerialization Serialization { get; set; }
        /// <summary>
        /// configuration name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// save configuration
        /// </summary>
        void Save();
        /// <summary>
        /// load configuration
        /// </summary>
        void Load();
    }
}
