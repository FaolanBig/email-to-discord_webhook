using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using Serilog;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace email_to_discord_webhook
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt")
                .CreateLogger();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("app.settings.json")
                .Build();
        }
    }
}
