using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace YukiApiCSharp {
    class Config {
        public static Config ReadConfig() {
            return new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build().Deserialize<Config>(File.ReadAllText("config.yaml"));
        }

        public Database Database;
        public Mail Mail;
        public Session Session;
        public Server Server;
    }
    struct Database {
        public Connection Connection;
        public TableItem[] Tables;
    }
    struct Connection {
        public string Host;
        public int Port;
        public string Database;
        public string Username;
        public string Password;
    }
    struct TableItem {
        public string Name;
        public ColumnItem[] Columns;
    }
    struct ColumnItem {
        public string Name;
        public string Type;
    }
    struct Mail {
        public string Server;
        public int Port;
        public string Username;
        public string Password;
        public string From;
    }
    struct Session {
        public string Key;
    }
    struct Server {
        public string Urls;
    }
}