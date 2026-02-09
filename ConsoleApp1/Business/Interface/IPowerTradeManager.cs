using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IPowerTradeManager
    {
        DataTable StartProcessingPowerTrades(string Headers, DateTime date);
        Task<DataTable> StartProcessingPowerTradesAsync(string Headers, DateTime date);
        
    }
}
