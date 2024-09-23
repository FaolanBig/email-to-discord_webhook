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
            const string jsonPath = "app.settings.json";
            try
            {
                string content = File.ReadAllText(jsonPath);
                EmailConfig config = JsonConvert.DeserializeObject<EmailConfig>(content);
            }
            catch (Exception ex)
            {
                ToLog.Err($"some shit went wrong when reading and deserializing app.settings.json - error: {ex.Message}");
            }
            
        }
    }
}
