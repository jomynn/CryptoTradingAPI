using CryptoTradingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using CryptoTradingAPI.Models;

namespace CryptoTradingAPI.Services
{
    public class TradingService
    {
        // This is a placeholder function for backtest logic
        // Path to the CSV file where records will be saved
        private const string RecordFilePath = "TradingRecords.csv";

        public List<BacktestPlotData> BacktestForChart()
        {
            var df = FetchData();  // Replace with actual data fetching logic
            CalculateHeikinAshi(df);  // Implement your Heikin-Ashi calculation here
            GenerateTradingSignals(df);  // Implement your signal generation here

            return BacktestLogicForChart(df);  // Process data for chart plotting
        }

        public string GetCurrentSignal()
        {
            // Fetch the most recent data (or the last row of data)
            var df = FetchData();  // Replace with actual data fetching logic
            CalculateHeikinAshi(df);  // Implement your Heikin-Ashi calculation here
            GenerateTradingSignals(df);  // Implement your signal generation here

            // Get the last row for current signal
            var lastRow = df.LastOrDefault();

            if (lastRow != null)
            {
                var currentSignal = lastRow["Signal"].ToString();
                return currentSignal;
            }
            else
            {
                return "No signal available";
            }
        }

        // Placeholder for actual data fetching (replace with Yahoo Finance API)
        private List<Dictionary<string, object>> FetchData()
        {
            // Sample data for testing
            return new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object> { { "Datetime", "2024-01-01 00:00" }, { "HA_Close", 50000.0 }, { "Signal", "open" } },
                new Dictionary<string, object> { { "Datetime", "2024-01-01 01:00" }, { "HA_Close", 51000.0 }, { "Signal", "close" } }
            };
        }

        private void CalculateHeikinAshi(List<Dictionary<string, object>> df)
        {
            // Implement your Heikin-Ashi calculation logic here
        }

        private void GenerateTradingSignals(List<Dictionary<string, object>> df)
        {
            // Implement your signal generation logic here
            foreach (var row in df)
            {
                // For simplicity, we are using a basic logic to decide signals.
                // Replace with your own strategy.
                row["Signal"] = "hold"; // Default signal is hold

                // Example: If Close price is rising, generate an open signal
                if (Convert.ToDouble(row["HA_Close"]) > 50000)
                {
                    row["Signal"] = "open";
                }

                // Example: If Close price is dropping, generate a close signal
                if (Convert.ToDouble(row["HA_Close"]) < 49000)
                {
                    row["Signal"] = "close";
                }
            }
        }

        private List<BacktestPlotData> BacktestLogicForChart(List<Dictionary<string, object>> df)
        {
            double balance = 1000;
            double position = 0;
            double entryPrice = 0;
            var plotData = new List<BacktestPlotData>();

            foreach (var row in df)
            {
                var signal = row["Signal"].ToString();
                var haClose = Convert.ToDouble(row["HA_Close"]);

                if (signal == "open" && position == 0)
                {
                    position = 0.1; // Example trade size
                    entryPrice = haClose;
                }
                else if (signal == "close" && position > 0)
                {
                    var exitPrice = haClose;
                    var profit = (exitPrice - entryPrice) * position;
                    balance += profit;
                    position = 0;
                }

                plotData.Add(new BacktestPlotData
                {
                    Datetime = row["Datetime"].ToString(),
                    Balance = balance
                });
            }

            return plotData;
        }

        public void SaveHourlyRecord()
        {
            // Get the latest trading signal (you may also want to get the latest balance here)
            var signal = GetCurrentSignal();  // Get the current signal ("open", "close", "hold")
            var balance = 1000;  // Replace with actual balance logic, if needed
            var currentTime = DateTime.UtcNow;

            // Prepare the trading record data
            var record = new TradingRecord
            {
                Datetime = currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Signal = signal,
                Balance = balance
            };

            // Append the record to the CSV file
            AppendRecordToCsv(record);
        }

        // Method to save the hourly record
        public void SaveHourlyRecord(string signal, double balance)
        {
            // Get current time in the required format
            var currentTime = DateTime.UtcNow;

            // Prepare the trading record data
            var record = new TradingRecord
            {
                Datetime = currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Signal = signal,
                Balance = balance
            };

            // Append the record to the CSV file
            AppendRecordToCsv(record);
        }

        // Method to append a record to the CSV file
        private void AppendRecordToCsv(TradingRecord record)
        {
            // Check if the file exists, if not create it and write the header
            bool fileExists = File.Exists(RecordFilePath);

            using (var writer = new StreamWriter(RecordFilePath, append: true))
            {
                if (!fileExists)
                {
                    // Write header if the file doesn't exist
                    writer.WriteLine("Datetime,Signal,Balance");
                }

                // Write the record data
                writer.WriteLine($"{record.Datetime},{record.Signal},{record.Balance}");
            }
        }

    }
}