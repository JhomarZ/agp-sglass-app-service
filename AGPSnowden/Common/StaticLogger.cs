
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace AGPSnowden.Common
{
    public class StaticLogger
    {
        public static void EnsureInitialized()
        {
            if (Log.Logger is not Serilog.Core.Logger)
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(LogEventLevel.Information)
                     .CreateLogger();
            }
        }
    }
}
