using Serilog;
using Tools.Interface;


namespace Business.Interface
{
    public  class Logger : ILogger
    {
        private  Serilog.ILogger _logger;

        public Logger(IConfigurationParameters configurationParameters) {

            string fullLogPath = Path.Combine(configurationParameters.LogFilePath,configurationParameters.LogfileName + ".log" );

            _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(fullLogPath,
            rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }
        public  void  Error(string message)
        {
            _logger.Error(message);
        }

        public void Info(string message)
        {
            _logger.Information(message);
        }

        public void Warn(string message)
        {
            _logger.Warning(message);
        }

    }
}
