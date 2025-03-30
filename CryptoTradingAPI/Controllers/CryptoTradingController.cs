using Microsoft.AspNetCore.Mvc;
using CryptoTradingAPI.Services;

namespace CryptoTradingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoTradingController : ControllerBase
    {
        private readonly TradingService _tradingService;

        public CryptoTradingController(TradingService tradingService)
        {
            _tradingService = tradingService;
        }

        [HttpGet("backtest_chart")]
        public IActionResult GetBacktestChart()
        {
            var chartData = _tradingService.BacktestForChart();
            return Ok(chartData);
        }

        [HttpGet("current_signal")]
        public IActionResult GetCurrentSignal()
        {
            var signal = _tradingService.GetCurrentSignal();
            return Ok(new { CurrentSignal = signal });
        }

        [HttpGet("hourly_record")]
        public IActionResult GetHourlyRecords()
        {
            try
            {
                var records = System.IO.File.ReadAllText("trading_records.csv");
                return Ok(records);
            }
            catch
            {
                return NotFound("No records found.");
            }
        }
    }
}
