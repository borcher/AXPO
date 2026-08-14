using Axpo;
using Business;
using Business.Interface;
using Business.Process;
using Data;
using Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Threading.Tasks;
using Tools.Configuration;
using Tools.Interface;


namespace ConsoleApp2
{
    internal class Program
    {
        static int intervalMs;
        static string? pahtInput;

        static async Task Main(string[]? args)
        {
            RequestInterval();
            RequestPath();

            var services = new ServiceCollection();

            // Cargamos la configuración
            var configBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            var toolsConfig = new Tools.Configuration.Configuration(configBuilder); // Ajusta al nombre real de tu clase lectora

            IConfigurationParameters cargadosParameters = toolsConfig.LoadSettings();

            services.AddSingleton<IConfigurationParameters>(cargadosParameters);

            services.AddTransient<ILogger, Logger>();
            services.AddTransient<IConfigurationBuilder, ConfigurationBuilder>();
            services.AddTransient<Tools.Interface.IConfiguration>(provider => toolsConfig);

            services.AddTransient<IConnection, Connection>();

            // Registrar MainManager como Singleton
            services.AddSingleton<MainManager>();

            // Registrar IProcessManager 
            services.AddTransient<IProcessManager>(provider =>
            {
                var mainManager = provider.GetRequiredService<MainManager>();
                var logger = provider.GetRequiredService<ILogger>();
                var connection = provider.GetRequiredService<IConnection>();

                return new ProcessManager(mainManager.ConfigurationParameters, logger, connection);
            });

            // Registrar ICsvManager 
            services.AddTransient<ICsvManager>(provider =>
            {
                var mainManager = provider.GetRequiredService<MainManager>();
                var logger = provider.GetRequiredService<ILogger>();

                return new CsvManager(logger, mainManager.ConfigurationParameters);
            });

            var serviceProvider = services.BuildServiceProvider();

            try
            {
                var mainManager = serviceProvider.GetRequiredService<MainManager>();
                var processManager = serviceProvider.GetRequiredService<IProcessManager>();
                var csvManager = serviceProvider.GetRequiredService<ICsvManager>();

                Console.WriteLine("Arrancando el sistema de procesamiento con Logs corregidos..." + Environment.NewLine);

                await mainManager.DoProcess(processManager, csvManager, intervalMs, pahtInput).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error crítico durante la ejecución: {ex.Message}" + Environment.NewLine);
            }
        }

        private static void RequestPath()
        {
            Console.WriteLine("Introduce path where store file(empty to use default one)" + Environment.NewLine);
            pahtInput = Console.ReadLine();
            if(string.IsNullOrEmpty(pahtInput))
            {
                Console.Write($"Selected default path" + Environment.NewLine);
                pahtInput = null;
            }
        }

        private static void RequestInterval()
        {
            Console.WriteLine("Introduce timer interval in minutes(<=0 Use configured)" + Environment.NewLine);
            var intervalInput = Console.ReadLine();

            if (string.IsNullOrEmpty(intervalInput) || int.Parse(intervalInput) <= 0)
            {
                Console.Write($"Selected default Interval" + Environment.NewLine);
                intervalMs =0;
            }
            else
            {
                Console.Write($"Selected Interval {intervalInput + Environment.NewLine} m interval");
                intervalMs = int.Parse(intervalInput) * 60 * 1000;
            }
        }
    }
}
