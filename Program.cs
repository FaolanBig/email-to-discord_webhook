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
using System.IO;
using MailKit;
using HtmlAgilityPack;

namespace email_to_discord_webhook
{
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            const string jsonPath = "app.settings.json";
            EmailConfig config = new();

            string content = File.ReadAllText(jsonPath);
            config = JsonConvert.DeserializeObject<EmailConfig>(content);

            try
            {
                using var client = new ImapClient();
                await client.ConnectAsync(config.imapServer, config.imapPort, true);
                await client.AuthenticateAsync(config.emailUserName, config.emailPW);
                await client.Inbox.OpenAsync(MailKit.FolderAccess.ReadWrite);

                foreach (var result in await client.Inbox.SearchAsync(SearchQuery.NotSeen))
                {
                    var message = await client.Inbox.GetMessageAsync(result);
                    bool found = false;

                    foreach (var accepted in config.emailAccepts)
                    {
                        if (string.Equals(message.From.Mailboxes.First().Address, accepted.emailAddress, StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (var item in accepted.data)
                            {
                                if (string.Equals(message.Subject, item.subjectShort, StringComparison.OrdinalIgnoreCase))
                                {
                                    string bodycontentHold;

                                    if (!string.IsNullOrEmpty(message.TextBody))
                                    {
                                        bodycontentHold = message.TextBody;
                                    }
                                    else if (!string.IsNullOrEmpty(message.HtmlBody))
                                    {
                                        bodycontentHold = HTML_toText(message.HtmlBody);
                                    }
                                    else
                                    {
                                        bodycontentHold = "ERROR: no body content found - please report to admin or mod";
                                    }

                                    await SendToDiscord(item.subjectShort, message.TextBody, item.webhookURL);
                                    found = true;
                                    ToLog.Inf($"Sent message successfully: from {accepted.emailAddress} to {item.webhookURL}");
                                }
                            }
                        }
                    }

                    if (!found)
                    {
                        await SendToDiscord(message.Subject, message.TextBody, config.defaultWebhookURL, "Unknown sender");
                        ToLog.Inf($"Sent message from unknown sender: {message.From} to {config.defaultWebhookURL}");
                    }

                    await client.Inbox.AddFlagsAsync(result, MessageFlags.Seen, true);
                }
            }
            catch (Exception ex)
            {
                ToLog.Err($"wrong shit at main - error: {ex.Message}");
            }
        }
        internal static async Task SendToDiscord(string subject, string content, string webhookURL, string extra = "")
        {
            if (!string.IsNullOrEmpty(extra)) { extra = extra + "\n" + subject; }
            else { extra = subject; }
            try
            {
                using var client = new HttpClient();
                var payload = new
                {
                    embeds = new[]
                    {
                    new
                    {
                        title = extra,
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
        internal static string HTML_toText(string html)
        {
            if (string.IsNullOrEmpty(html)) { return string.Empty; }
            var toConvert = new HtmlDocument();
            toConvert.LoadHtml(html);
            return toConvert.DocumentNode.InnerText;
        }
    }
}
