using Business.Configuration;
using Business.Interface;
using Business.Process;
using Microsoft.Extensions.Configuration;
using Ninject;
using Ninject.Parameters;
using System.Reflection;
using System.Timers;

namespace ConsoleApp2
{
    internal class Program
    {
        static IConfigurationBuilder _configurationBuilder = new ConfigurationBuilder();
        static Business.Interface.IConfiguration _configuration = new Configuration(_configurationBuilder);
        static ConfigurationParameters _configurationParameters = _configuration.LoadSettings();
        static ConstructorArgument configurationParameters = new ConstructorArgument("configurationParameters", _configurationParameters);
        static System.Timers.Timer timer;
        static IKernel kernel = new StandardKernel();
        static ILogger logger;
        static IExcelManager excel;
        static IProcessManager ProcessManager;


        static void Main(string[]? args)
        {
            
            kernel.Load(Assembly.GetExecutingAssembly());
            logger = kernel.Get<ILogger>(configurationParameters);
            var _logger = new ConstructorArgument("configurationParameters", logger);
            excel = kernel.Get<IExcelManager>(configurationParameters);
            ProcessManager = kernel.Get<IProcessManager>(configurationParameters, _logger);

            InitProcess();
        }

        private static void InitProcess()
        {
            using (timer = new System.Timers.Timer(_configurationParameters.TimeInterval))
            {
                timer.Start();
                timer.Elapsed += OnTimedEvent;
                ProcessManager.Start();
                Console.ReadLine();
            }
        }

        private static void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            timer.Stop();
            InitProcess();
        }
    }
}
