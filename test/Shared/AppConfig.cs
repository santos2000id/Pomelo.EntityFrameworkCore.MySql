using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Pomelo.EntityFrameworkCore.MySql.Tests
{
    public static class AppConfig
    {
        public static readonly int EfBatchSize =
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("EF_BATCH_SIZE"))
                ? Convert.ToInt32(Environment.GetEnvironmentVariable("EF_BATCH_SIZE"))
                : 1;

        public static readonly int EfRetryOnFailure =
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("EF_RETRY_ON_FAILURE"))
                ? Convert.ToInt32(Environment.GetEnvironmentVariable("EF_RETRY_ON_FAILURE"))
                : 0;

        public static readonly string EfDatabase = Environment.GetEnvironmentVariable("EF_DATABASE");
        public static readonly uint? EfPort = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("EF_PORT"))
            ? (uint?)Convert.ToUInt32(Environment.GetEnvironmentVariable("EF_PORT"))
            : null;

        public static string ConnectionString
        {
            get
            {
                // POMELO_TEST_CONNECTION takes precedence, so credentials can stay out of config.json.
                var connectionString = Environment.GetEnvironmentVariable("POMELO_TEST_CONNECTION");
                var csb = new MySqlConnectionStringBuilder(
                    string.IsNullOrWhiteSpace(connectionString) ? Config["Data:ConnectionString"] : connectionString);

                if (EfPort.HasValue)
                {
                    csb.Port = EfPort.Value;
                }

                return csb.ConnectionString;
            }
        }

        public static ServerVersion ServerVersion => _lazyServerVersion.Value;

        private static readonly Lazy<ServerVersion> _lazyServerVersion = new Lazy<ServerVersion>(() =>
        {
            var serverVersionString = Environment.GetEnvironmentVariable("SERVER_VERSION");

            if (string.IsNullOrEmpty(serverVersionString))
            {
                serverVersionString = Config["Data:ServerVersion"];
            }

            return string.IsNullOrEmpty(serverVersionString) ||
                   string.Equals(serverVersionString, "auto", StringComparison.OrdinalIgnoreCase)
                ? ServerVersion.AutoDetect(ConnectionString)
                : ServerVersion.Parse(serverVersionString);
        });

        public static IConfigurationRoot Config => _lazyConfig.Value;

        private static readonly Lazy<IConfigurationRoot> _lazyConfig = new Lazy<IConfigurationRoot>(() =>
            new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath))
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("config.json")
                .Build());
    }
}
