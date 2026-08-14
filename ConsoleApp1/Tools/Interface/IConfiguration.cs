using Tools.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Interface
{
    public interface IConfiguration
    {
        ConfigurationParameters LoadSettings();
    }
   
}
