using Huarui.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huarui
{
    class FTPLis : ILisService
    {
        public string Type => "FTP";

        public IConfiguration Configuration => new FTPLisConfiguration();

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
