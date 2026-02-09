using Business.Configuration;
using Business.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Timers;
using IConfiguration = Business.Interface.IConfiguration;
using ILogger = Business.Interface.ILogger;

namespace Business.Process
{
    public class ProcessManager(PowerTradeManager process, IExcelManager excelExport, 
        IConfigurationParameters configurationParameters, ILogger logger) : IProcessManager
    {
        private IConfigurationParameters _configurationParameters = configurationParameters;
        private IPowerTradeManager _process = process;
        private IExcelManager _excelExport = excelExport;
        private ILogger _logger = logger;

        #region Synchronous
        public void Start()
        {
            try
            {
                _logger.Info(string.Format("{1} - Starting Process at {0}", DateTime.Now,this.GetType().ToString()));
                StartProcessing();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{1} - An exception has happened: {0}", ex.Message, this.GetType().ToString()));
                StartProcessing();
            }
        }

        private void StartProcessing()
        {
            var date = DateTime.Parse(DateTime.Now.ToString(_configurationParameters.DateFormat), CultureInfo.InvariantCulture);
            try
            {
                _logger.Info(string.Format("{1} - {0}:Starting Process", DateTime.Now, this.GetType().ToString()));
                var data = _process.StartProcessingPowerTrades(_configurationParameters.HeadersTitle, date);
                if (data != null && data.Rows.Count > 0)
                {
                    _logger.Info(string.Format("{1} - {0}:Exporting to Excel", DateTime.Now, this.GetType().ToString()));
                    _excelExport.ExportToExcel(data);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{2} - {0}:{1}", DateTime.Now, ex.Message, this.GetType().ToString()));
                StartProcessing();
            }
        }
        #endregion

        #region Asynchronous

        public async Task StartAsync()
        {
            try
            {
                _logger.Info(string.Format("{1} - Starting Process at {0}", DateTime.Now, this.GetType().ToString()));
                await StartProcessAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{1} - An exception has happened: {0}", ex.Message, this.GetType().ToString()));
                await StartProcessAsync();
            }
        }

        private async Task StartProcessAsync()
        {
            var date = DateTime.Parse(DateTime.Now.ToString(_configurationParameters.DateFormat), CultureInfo.InvariantCulture);
            try
            {
                _logger.Info(string.Format("{1} - {0}:Starting Process", DateTime.Now, this.GetType().ToString()));
                var data = await _process.StartProcessingPowerTradesAsync(_configurationParameters.HeadersTitle, date);
                if (data != null && data.Rows.Count > 0)
                {
                    _logger.Info(string.Format("{1} - {0}:Exporting to Excel", DateTime.Now, this.GetType().ToString()));
                    _excelExport.ExportToExcel(data);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("{2} - {0}:{1}", DateTime.Now, ex.Message, this.GetType().ToString()));
                await StartProcessAsync();
            }
        }

        #endregion

    }
}
