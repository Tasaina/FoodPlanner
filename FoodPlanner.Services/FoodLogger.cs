using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace FoodPlanner.Services
{
    public class FoodLogger
    {
        private readonly Logger _logger;

        public FoodLogger()
        {
            var logPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FoodPlanner_Logs", "Log.txt");
            _logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(logPath, LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: false)
                .CreateLogger();
        }

        public void Log(LogEventLevel level, string message)
        {
            _logger.Write(level, message);
        }
    }
}