using Axpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IConnection
    {
        Task<IEnumerable<PowerTrade>> GetTradesAsync(DateTime date);
    }
}
