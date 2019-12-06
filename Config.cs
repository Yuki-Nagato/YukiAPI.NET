using System.IO;
using System.Text.Json;

namespace YukiAPI {
    public class Config {
        public HttpsConfig Https { get; set; }
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
}