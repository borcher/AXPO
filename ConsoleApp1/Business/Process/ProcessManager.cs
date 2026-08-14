using Axpo;
using Business.Interface;
using Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using Tools.Interface;
using ILogger = Business.Interface.ILogger;


namespace Business.Process
{
    public class ProcessManager: IProcessManager
    {
        private readonly IConfigurationParameters _configurationParameters;
        private readonly ILogger _logger; 
        private readonly IConnection _connection;

        public ProcessManager(IConfigurationParameters configurationParameters, ILogger logger, IConnection connection)
        {
            _configurationParameters = configurationParameters;
            _logger = logger;
            _connection = connection;
        }
        #region Synchronous

        public async Task<DataTable> StartProcessing()
        {
            _logger.Info($"{this.GetType()} - Starting Process at {DateTime.Now}");
            var date = CreateUTCTime();
            try
            {
                _logger.Info($"{this.GetType()} - {DateTime.Now}: Starting Process");
                var formattedDate = date.ToString("yyyy-MM-dd");

                var powerTradeCollection = await _connection.GetTradesAsync(date).ConfigureAwait(false);
                if (powerTradeCollection != null && powerTradeCollection.Any())
                {
                    return await ProcessResult(powerTradeCollection.ToList()).ConfigureAwait(false);
                }
                return CreateDataTable();
            }
            catch (Exception ex)
            {
                _logger.Error($"{this.GetType()} - {DateTime.Now}: Error crítico: {ex.Message} -> {ex.StackTrace}");
                return CreateDataTable();
            }
        }

        #endregion

        private async Task<DataTable> ProcessResult(List<PowerTrade> powerTrades)
        {
            _logger.Info($"{this.GetType()} - Create table data at {DateTime.Now}");
            DataTable dt = CreateDataTable();
            _logger.Info($"{this.GetType()} - Create Data Table with results at {DateTime.Now}");

            foreach (PowerTrade powerTrade in powerTrades)
            {
                if (dt.Rows.Count > 0)
                {
                    if (powerTrade.Periods != null)
                    {
                        foreach (var period in powerTrade.Periods)
                        {
                            var row = dt.AsEnumerable().Where(p => p[2].ToString() == period.Period.ToString()).FirstOrDefault();
                            if (row != null)
                            {
                                row[1] = double.Parse(row[1].ToString(), System.Globalization.CultureInfo.InvariantCulture) + period.Volume;
                            }
                        }
                    }
                }
                else
                {
                    await CreateInitialRowOfData(dt, powerTrade).ConfigureAwait(false);
                }
            }
            return dt;
        }

        private static async Task CreateInitialRowOfData(DataTable dt, PowerTrade powerTrade)
        {
            var time = new TimeOnly(23, 00);
            if (powerTrade.Periods == null) return;
            foreach (var value in powerTrade.Periods)
            {
                var dr = dt.NewRow();
                dr[0] = time;
                dr[1] = value.Volume;
                dr[2] = value.Period;
                dt.Rows.Add(dr);
                time = time.AddHours(1);
            }
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            var headers = _configurationParameters.HeadersTitle.Split(';');
            dt.Columns.Add(headers[0]);
            dt.Columns.Add(headers[1]);
            dt.Columns.Add("Period");
            return dt;
        }

        private DateTime CreateUTCTime()
        {
            DateTime horaUtc = DateTime.UtcNow; 
            return horaUtc.Date;
        }
    }
}
