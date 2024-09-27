/*
   etdWebhook is a console-based message-forwarding tool. 
   etdWebhook forwards text_bodies (email) to a discord webhook. 

   Copyright (C) 2024  Carl Öttinger (Carl Oettinger)

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU Affero General Public License as published
   by the Free Software Foundation, either version 3 of the License, or
   any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU Affero General Public License for more details.

   You should have received a copy of the GNU Affero General Public License
   along with this program.  If not, see <https://www.gnu.org/licenses/>.

   You can contact me in the following ways:
       EMail: oettinger.carl@web.de or big-programming@web.de

   issues and bugs can be reported on https://guthub.com/faolanbig/etdwebhook/issues/new
*/


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
