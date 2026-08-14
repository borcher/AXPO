using Tools.Interface;

namespace Tools.Configuration
{
    public class ConfigurationParameters: IConfigurationParameters
    {
        public string OutputFileName { get; set; }
        public string OutputFileNameDateFormat { get; set; }
        public string OutputFileNameTimeFormat { get; set; }
        public string OutputSavePath { get; set; }
        public decimal TimeInterval { get; set; }
        public string HeadersTitle { get; set; }
        public string LogFilePath { get; set; }
        public string LogfileName { get; set;}
        public string DateFormat {  get; set; }
        public string BaseUrlhttp { get; set; }
        public string BaseUrlhttps { get; set; }
    }
}