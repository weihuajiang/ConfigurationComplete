using Huarui.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huarui
{
    class ASTMLis : ILisService
    {
        public string Type => "ASTM";

        public IConfiguration Configuration => new ASTMLisConfiguration();

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
