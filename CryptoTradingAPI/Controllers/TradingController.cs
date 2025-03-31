using CryptoTradingAPI.Models;
using CryptoTradingAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradingController : ControllerBase
    {
        private readonly TradingService _tradingService;

        public TradingController(TradingService tradingService)
        {
            _tradingService = tradingService;
        }

        // API endpoint to manually save a trading record (for testing)
        [HttpPost("save-record")]
        public IActionResult SaveRecord([FromQuery] string signal, [FromQuery] double balance)
        {
            // Call the service to save the record
            _tradingService.SaveHourlyRecord(signal, balance);
            return Ok("Record saved successfully.");
        }


        [HttpGet("backtest")]
        public IActionResult GetBacktestData()
        {
            var random = new Random();
            var backtestData = new List<TradeRecord>();

            double balance = 1000;  // Starting balance
            double btcPrice = 50000; // Initial BTC price
            double entryPrice = 0; // Price at which trade was entered
            bool hasPosition = false;
            DateTime startDate = DateTime.UtcNow.AddMonths(-3);
            DateTime currentDate = startDate;

            while (currentDate <= DateTime.UtcNow)
            {
                // Simulate price changes (±2%)
                btcPrice *= (1 + (random.NextDouble() * 0.04 - 0.02));

                // Random buy/sell decision (50% probability)
                string action = random.Next(2) == 0 ? "BUY" : "SELL";
                double exitPrice = btcPrice;
                double profit = 0;

                if (action == "BUY" && !hasPosition)
                {
                    entryPrice = btcPrice;
                    hasPosition = true;
                }
                else if (action == "SELL" && hasPosition)
                {
                    profit = exitPrice - entryPrice;  // Profit Calculation
                    balance += profit;
                    hasPosition = false;
                }

                // Add trade record
                backtestData.Add(new TradeRecord
                {
                    Datetime = currentDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Action = action,
                    EntryPrice = entryPrice,
                    ExitPrice = hasPosition ? 0 : exitPrice,
                    Profit = profit,
                    Balance = balance
                });

                currentDate = currentDate.AddHours(1);  // Move forward by 1 hour
            }

            return Ok(backtestData);
        }
    }
}


