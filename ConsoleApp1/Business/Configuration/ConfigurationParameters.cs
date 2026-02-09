

using Business.Interface;

namespace Business.Configuration
{
    public class ConfigurationParameters: IConfigurationParameters
    {
        public string OutputFileName { get; set; }
        public string OutputFileNameDateFormat { get; set; }
        public string OutputFileNameTimeFormat { get; set; }
        public string OutputSavePath { get; set; }
        public int TimeInterval { get; set; }
        public string HeadersTitle { get; set; }
        public string LogFilePath { get; set; }
        public string LogfileName { get; set;}
        public string DateFormat {  get; set; }
    }
}