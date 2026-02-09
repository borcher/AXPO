using Axpo;

namespace Data
{
    public static class Connection
    {
        public static IEnumerable<PowerTrade> GetTrades(DateTime date) {
            PowerService p=new PowerService();    
             return p.GetTrades(date);   
        }

        public async static Task<IEnumerable<PowerTrade>> GetTradesAsync(DateTime date)
        {
            PowerService p = new PowerService();
            return await p.GetTradesAsync(date);
        }
    }
}
