using System.IO;
using System.Text.Json;

namespace YukiAPI {
    public class Config {
        public HttpsConfig Https { get; set; }
        public EmailConfig Email { get; set; }
        public static Config Read() {
            return JsonSerializer.Deserialize<Config>(File.ReadAllText("PrivateData/Configurations/config.json"), new JsonSerializerOptions() {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
    public struct HttpsConfig {
        public CertificateConfig Certificate { get; set; }
    }
    public struct CertificateConfig {
        public string File { get; set; }
        public string Password { get; set; }
    }
    public struct EmailConfig {
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
    }
}