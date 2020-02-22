using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using MimeKit;
using MailKit.Net.Smtp;

namespace YukiAPI {
    public class YukiController<TSelf> : ControllerBase {
        protected readonly ILogger<TSelf> Logger;
        public YukiController(ILogger<TSelf> logger) {
            Logger = logger;
        }
    }

    public static class Helper {
        public static string ToHttpString(this IHeaderDictionary header) {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, StringValues> i in header) {
                foreach (string j in i.Value) {
                    sb.Append(i.Key).Append(": ").Append(j).Append("\r\n");
                }
            }
            return sb.ToString();
        }

        public static void SendEmail(InternetAddress to, string subject, string body) {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(Config.Read().Email.FromName, Config.Read().Email.FromAddress));
            message.To.Add(to);
            message.Subject = subject;
            message.Body = new TextPart("plain") {
                Text = body
            };

            using (var client = new SmtpClient()) {
                client.CheckCertificateRevocation = false; // Linux 不写就用不了
                client.Connect(Config.Read().Email.SmtpHost, Config.Read().Email.SmtpPort, true);
                client.Authenticate(Config.Read().Email.SmtpUsername, Config.Read().Email.SmtpPassword);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}