using Axpo;
using Business.Configuration;
using Business.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeDataExports;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Business.Process
{
    public  class ExcelManager: IExcelManager
    {
        private ILogger _logger;
        private IDataWorkbook workbook;
        private IConfigurationParameters _configurationParameters;
        private IDataWorksheet dataPowerTrade;
        private string path; 

        public ExcelManager(ILogger logger, IConfigurationParameters configurationParameters) {
            _logger = logger;
            _configurationParameters = configurationParameters;
        }

        public void ExportToExcel (DataTable data)
        { 
            _logger.Info(string.Format("{0} -  Load data in Excel file at {1}", this.GetType().ToString(),DateTime.Now));
            path = CreatePath();
            CreateWorkSheet(data);
            SaveFile();
        }

        private void SaveFile()
        {
            workbook.GetBytes();
            _logger.Info(string.Format("{1} - Saving Excel file in {0}", path, this.GetType().ToString()));
            workbook.Save(path);
            _logger.Info((string.Format("{1} - Data Saved on {0}", path, this.GetType().ToString())));
        }

        private string CreatePath()
        {
            string filename = _configurationParameters.OutputFileName;
            filename = string.Format(filename, DateTime.Now.ToString(_configurationParameters.OutputFileNameDateFormat), DateTime.Now.ToString(_configurationParameters.OutputFileNameTimeFormat)) + ".xlsx";
            var path = _configurationParameters.OutputSavePath + filename;
            return path;
        }

        private void CreateWorkSheet(DataTable data)
        {
            if (!File.Exists(path))
            {
                workbook = new DataExport().CreateXLSX2019();
            }
            workbook.FontSize = 11;
            _logger.Info(string.Format("{0} - Create Excel file at {1}",this.GetType().ToString(),DateTime.Now));
            // Create worksheets
            dataPowerTrade = workbook.AddWorksheet(string.Format("DataPowerTrade_{0}",DateTime.Now.ToString("hhmmss")));
            dataPowerTrade.AddRow()
                 .AddCell(_configurationParameters.HeadersTitle.Split(';')[0], DataType.String)
                 .AddCell(_configurationParameters.HeadersTitle.Split(';')[1], DataType.String);
            foreach (DataRow row in data.Rows)
            {
                dataPowerTrade.AddRow()
                    .AddCell(row[0], DataType.String)
                    .AddCell(row[1], DataType.Number);
            }
        }
    }
}
