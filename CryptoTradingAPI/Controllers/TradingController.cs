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
    }
}
