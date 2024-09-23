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
