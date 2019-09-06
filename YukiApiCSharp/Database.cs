using Npgsql;
using System;

namespace YukiApiCSharp {
    public class DatabaseCommandConnection : IDisposable {
        static string ConnectionString {
            get {
                Config config = Config.ReadConfig();
                Connection connection = config.Database.Connection;
                if(connection.Host.StartsWith('/')) {
                    return $"Host={connection.Host}; Database={connection.Database}";
                }
                else {
                    return $"Host={connection.Host}; Port={connection.Port}; Username={connection.Username}; Password={connection.Password}; Database={connection.Database}";
                }
            }
        }

        readonly NpgsqlConnection connection;
        public NpgsqlCommand Command { get; }

        public DatabaseCommandConnection(string cmdText) {
            connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            Command = new NpgsqlCommand(cmdText, connection);
        }

        public void Dispose() {
            Command.Dispose();
            connection.Close();
            connection.Dispose();
        }

        ~DatabaseCommandConnection() {
            Dispose();
        }
    }
}