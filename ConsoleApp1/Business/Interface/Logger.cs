using Business.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.Interface
{
    public  class Logger : ILogger
    {
        private  Serilog.ILogger _logger;

        public Logger(IConfigurationParameters configurationParameters) {
            _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(configurationParameters.LogFilePath + configurationParameters.LogfileName + ".log",
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
