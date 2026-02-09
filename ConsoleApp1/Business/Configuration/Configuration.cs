using Business.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Configuration
{
    public class Configuration: Interface.IConfiguration
    {
        private IConfigurationBuilder _configurationBuilder;
        public Configuration(IConfigurationBuilder configurationBuilder) {
            _configurationBuilder = configurationBuilder;
        }

        public  ConfigurationParameters LoadSettings()
        {
            var configuration = _configurationBuilder.AddJsonFile("Configuration.json").Build();

            return new ConfigurationParameters()
            {
                OutputFileName= configuration["FileName"],
                OutputFileNameDateFormat = configuration["FileNameDateFormat"],
                OutputFileNameTimeFormat = configuration["FileNameTimeFormat"],
                OutputSavePath = configuration["SavePath"],
                TimeInterval = int.Parse(configuration["TimeInterval"]),
                HeadersTitle= configuration["Headers"],
                LogfileName =string.Format(configuration["LogFileName"], DateTime.Now.ToString(configuration["FileNameDateFormat"])),
                LogFilePath = configuration["LogFilePath"],
                DateFormat = configuration["DateFormat"]
            };
        }
       
    }


}
