using Npgsql;
using System;

namespace YukApiCSharp {
    public class DatabaseCommandConnection : IDisposable {
        static string ConnectionString {
            get {
                using (Config.ConfigNode node = Config.OpenFileNode()) {
                    Config.ConfigNode databaseNode = node["database"]["connection"];
                    return $"Host={databaseNode["host"]}; Username={databaseNode["username"]}; Password={databaseNode["password"]}; Database={databaseNode["database"]}";
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
