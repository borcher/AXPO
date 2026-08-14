using Microsoft.Extensions.Configuration;


namespace Tools.Configuration
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
                DateFormat = configuration["DateFormat"],
                BaseUrlhttp = configuration["BaseUrlhttp"],
                BaseUrlhttps = configuration["BaseUrlhttps"]
            };
        }
        public ConfigurationParameters LoadURLSettings()
        {
            var configuration = _configurationBuilder.AddJsonFile("Configuration.json").Build();

            return new ConfigurationParameters()
            {
                TimeInterval = int.Parse(configuration["TimeInterval"]),
                BaseUrlhttp = configuration["BaseUrlhttp"],
                BaseUrlhttps = configuration["BaseUrlhttps"]
            };
        }


    }


}
