using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoTradingAPI.Services
{
    public class SaveRecordService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly TradingService _tradingService;

        public SaveRecordService(TradingService tradingService)
        {
            _tradingService = tradingService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Set up the timer to call SaveHourlyRecord every hour (3600000 ms)
            _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private void ExecuteTask(object state)
        {
            _tradingService.SaveHourlyRecord();  // Call the method to save record every hour
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

