using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace email_to_discord_webhook
{
    internal static class ToLog
    {
        internal static void Inf(string message)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt")
                .CreateLogger();
            Log.Information(message);
            Log.CloseAndFlush();
        }
        internal static void Err(string message)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt")
                .CreateLogger();
            Log.Error(message);
            Log.CloseAndFlush();
        }
    }
}
