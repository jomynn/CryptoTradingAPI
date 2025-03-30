namespace CryptoTradingAPI.Models
{
    public class TradeRecord
    {
        public string Datetime { get; set; }
        public string Action { get; set; }
        public double EntryPrice { get; set; }
        public double ExitPrice { get; set; }
        public double Profit { get; set; }
        public double Balance { get; set; }
    }
}
