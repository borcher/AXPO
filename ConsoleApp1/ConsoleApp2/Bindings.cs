using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Ninject;
using Business.Interface;
using Business.Process;
using Business.Configuration;
using Microsoft.Extensions.Configuration;
using IConfiguration = Business.Interface.IConfiguration;
namespace ConsoleApp1
{
  

    public class Bindings : NinjectModule
    {
        public override void Load()
        {

            Bind<IConfiguration>().To<Configuration>().InSingletonScope();
            Bind<IConfigurationBuilder>().To<ConfigurationBuilder>().InSingletonScope();
            Bind<IConfigurationParameters>().To<ConfigurationParameters>().InSingletonScope(); 
            Bind<ILogger>().To<Logger>().InSingletonScope();
            Bind<IExcelManager>().To<ExcelManager>().InSingletonScope();
            Bind<IPowerTradeManager>().To<PowerTradeManager>().InSingletonScope();
            Bind<IProcessManager>().To<ProcessManager>().InSingletonScope();
        }
    
    }
}
