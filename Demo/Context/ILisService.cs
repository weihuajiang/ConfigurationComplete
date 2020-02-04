using Huarui.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huarui
{
    /// <summary>
    /// LIS service
    /// </summary>
    public interface ILisService : IService
    {
        string Type { get; }
        IConfiguration Configuration { get; }
    }
}
