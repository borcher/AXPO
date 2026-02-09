using Business.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IConfiguration
    {
        ConfigurationParameters LoadSettings();
    }
   
}
