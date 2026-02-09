using Axpo;
using Business.Configuration;
using Business.Interface;
using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Business.Process
{
    public class PowerTradeManager:IPowerTradeManager
    {
        string _headers = string.Empty;
        ILogger _logger;
        public PowerTradeManager(ILogger logger)
        {
            _logger = logger;
        }

        #region Sync

        public DataTable StartProcessingPowerTrades(string Headers, DateTime date) {
            _headers = Headers;
           return ProcessResult(GetPowerTrades(date));
        }

        private List<PowerTrade> GetPowerTrades(DateTime date)
        {
            _logger.Info(string.Format("{1} - Getting List of Power Trades at  {0}", DateTime.Now.ToString(),this.GetType().ToString()));
            return Connection.GetTrades(date).ToList();
        }

        #endregion

        #region Asynch

        public async Task<DataTable> StartProcessingPowerTradesAsync(string Headers, DateTime date)
        {
            _headers = Headers;
            return ProcessResult(await GetPowerTradesAsync(date));
        }

        private Task<List<PowerTrade>> GetPowerTradesAsync(DateTime date)
        {
            _logger.Info(string.Format("{1} - Getting List of Power Trades at  {0}", DateTime.Now.ToString(), this.GetType().ToString()));
            return Task.FromResult(Connection.GetTradesAsync(date).Result.ToList());
        }

        #endregion

        private DataTable ProcessResult(List<PowerTrade> powerTrades)
        {
            _logger.Info(string.Format("{1} - Create table data at  {0}", DateTime.Now.ToString(), this.GetType().ToString()));
            DataTable dt = CreateDataTable();
            _logger.Info(string.Format("{0} - Create Data Table with results at {1} ", this.GetType().ToString(), DateTime.Now));
            foreach (PowerTrade powerTrade in powerTrades)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (var period in powerTrade.Periods)
                    {
                        var row = dt.AsEnumerable().Where(p => p[2].ToString() == period.Period.ToString()).FirstOrDefault();
                        if (row != null)
                        {
                            row[1] = double.Parse(row[1].ToString()) + period.Volume;
                        }
                    }
                }
                else
                {
                    CreateInitialRowOfData(dt, powerTrade);
                }
            }
            return dt;
        }

        private static void CreateInitialRowOfData(DataTable dt, PowerTrade powerTrade)
        {
            var time = new TimeOnly(23, 00);
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
            dt.Columns.Add(_headers.Split(';')[0]);
            dt.Columns.Add(_headers.Split(';')[1]);
            dt.Columns.Add("Period");
            return dt;
        }
    }
}
