using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace email_to_discord_webhook
{
    public class JsonHandler
    {
        public string jsonPath;

        public JsonHandler() { }

        public void init()
        {
            string content = File.ReadAllText(jsonPath);
            EmailConfig config = JsonConvert.DeserializeObject<EmailConfig>(content);
        }
    }
    public class Data
    {
        public string subjectShort { get; set; }
        public string webhookURL { get; set; }
    }
    public class EmailAccept
    {
        public string emailAddress { get; set; }
        public List<Data> data { get; set; }
    }
    public class EmailConfig
    {
        public string imapServer { get; set; }
        public int imapPort { get; set; }
        public string emailUserName { get; set; }
        public string emailPW { get; set; }
        public string emailRecipient { get; set; }
        public string defaultWebhookURL { get; set; }

        public List<EmailAccept> emailAccepts { get; set; }
    }
}
