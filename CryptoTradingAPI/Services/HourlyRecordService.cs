using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using CryptoTradingAPI.Services;

namespace CryptoTradingAPI.Services
{
    public class HourlyRecordService : BackgroundService
    {
        private readonly TradingService _tradingService;

        public HourlyRecordService(TradingService tradingService)
        {
            _tradingService = tradingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Run the method every hour
                _tradingService.SaveHourlyRecord();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Wait 1 hour before next execution
            }
        }
    }
}
