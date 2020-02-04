using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huarui.Config
{
    /// <summary>
    /// configuration serialization
    /// </summary>
    public interface IConfigurationSerialization
    {
        /// <summary>
        /// save configuration
        /// </summary>
        /// <param name="config">configuration object</param>
        void Save(IConfiguration config);

        /// <summary>
        /// Load configuration
        /// </summary>
        /// <param name="t">configuration type</param>
        /// <returns>configuration object</returns>
        IConfiguration Load(Type t);

        /// <summary>
        /// load configuration with type
        /// </summary>
        /// <typeparam name="T">configuration Type</typeparam>
        /// <returns>configuration object with type</returns>
        T Load<T>() where T : IConfiguration;
    }
}
