using Axpo;
using Tools.Configuration;
using Tools.Interface;
using Business.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeDataExports;
using static System.Runtime.InteropServices.JavaScript.JSType;
using FreeDataExports.Delimited;

namespace Business.Process
{
    public  class CsvManager: ICsvManager
    {
        private ILogger _logger;
        private IDataWorkbook workbook;
        private IConfigurationParameters _configurationParameters;
        private IDataWorksheet dataPowerTrade;
        private Csv dataExported;
        private string path; 

        public CsvManager(ILogger logger, IConfigurationParameters configurationParameters) {
            _logger = logger;
            _configurationParameters = configurationParameters;
        }

        public async Task ExportToExcel (DataTable data, string userPath)
        { 
            _logger.Info(string.Format("{0} -  Load data in Excel file at {1}", this.GetType().ToString(),DateTime.Now));
            await ManagePath(userPath);
            await CreateWorkSheet(data);
            await SaveFile();
        }

        private async Task ManagePath(string userPath)
        {
            string filename = _configurationParameters.OutputFileName;
            filename = string.Format(filename, DateTime.Now.ToString(_configurationParameters.OutputFileNameDateFormat), 
                DateTime.Now.ToString(_configurationParameters.OutputFileNameTimeFormat)) + ".csv";

            if (string.IsNullOrEmpty(userPath))
            {
                path = _configurationParameters.OutputSavePath + filename;
            }
            else
            {
                path = userPath + filename;
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private async Task CreateWorkSheet(DataTable data)
        {
            if (!File.Exists(path))
            {
                dataExported = new DataExport().CreateCsv();
            }
            string[] title = _configurationParameters.HeadersTitle.Split(';');
            dataExported.AddRow(title[0], title[1]);
            foreach (DataRow row in data.Rows)
            {
                dataExported.AddRow(row[0], row[1]);
            }
        }
        
        private async Task SaveFile()
        {
            _logger.Info(string.Format("{1} - Saving Excel file in {0}", path, this.GetType().ToString()));
            dataExported.Save(path);
            _logger.Info((string.Format("{1} - Data Saved on {0}", path, this.GetType().ToString())));
        }
    }
}
