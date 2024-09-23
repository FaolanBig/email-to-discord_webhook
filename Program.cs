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
        internal static async Task Main(string[] args)
        {
            const string jsonPath = "app.settings.json";
            EmailConfig config = new();
            try
            {
                string content = File.ReadAllText(jsonPath);
                config = JsonConvert.DeserializeObject<EmailConfig>(content);
                ToLog.Inf("read contents of app.settings.json");
            }
            catch (Exception ex)
            {
                ToLog.Err($"some shit went wrong when reading and deserializing app.settings.json - error: {ex.Message}");
                Environment.Exit(1);
            }

            try
            {
                using var client = new ImapClient();
                await client.ConnectAsync(config.imapServer, config.imapPort, true);
                await client.AuthenticateAsync(config.emailUserName, config.emailPW);
                await client.Inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

                foreach (var accepted in config.emailAccepts)
                {
                    var query = SearchQuery.FromContains(accepted.emailAddress).And(SearchQuery.ToContains(config.emailRecipient));
                    var results = await client.Inbox.SearchAsync(query);

                    foreach (var result in results)
                    {
                        var message = await client.Inbox.GetMessageAsync(result);
                        if (string.Equals(message.Subject, accepted.data.subjectShort, StringComparison.OrdinalIgnoreCase))
                        {
                            await SendToDiscord(accepted.data.subjectShort, message.TextBody, accepted.data.webhookURL);
                            ToLog.Inf($"send message successfully");
                            ToLog.Inf($"message data: from: {accepted.emailAddress} to: {accepted.data.webhookURL}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ToLog.Err($"wrong shit ad main - error: {ex.Message}");
            }
        }
        internal static async Task SendToDiscord(string subject, string content, string webhookURL)
        {
            try
            {
                using var client = new HttpClient();
                var payload = new
                {
                    embeds = new[]
                    {
                    new
                    {
                        title = subject,
                        description = content,
                        color = 0xFF007F,
                        footer = new
                        {
                            text = "this is an automated message\ndetails available on https://github.com/FaolanBig/etdWebhook/wiki"
                        },
                        timestamp = DateTime.Now
                    }
                }
                };

                var jsonPayload = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                    );
                var response = await client.PostAsync(webhookURL, jsonPayload);

                if (!response.IsSuccessStatusCode)
                {
                    ToLog.Err($"shit went wrong while sending to Discord - error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                ToLog.Err($"accident at SendToDiscord - error: {ex.Message}");
            }
        }
    }
}
