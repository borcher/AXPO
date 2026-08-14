using Axpo;
using Data.Interfaces;

namespace Data
{
    public class Connection :IConnection
    {
        public async Task<IEnumerable<PowerTrade>> GetTradesAsync(DateTime date)
        {
            PowerService p = new PowerService();
            var test=await p.GetTradesAsync(date);
            return test;
        }
    }
}
