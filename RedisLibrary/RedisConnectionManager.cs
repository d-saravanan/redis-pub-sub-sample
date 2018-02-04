using Common;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisLibrary
{
    /// <summary>
    /// The Redis Cache Connection Manager
    /// </summary>
    public static class RedisCacheConnectionManager
    {
        static readonly string redisConnectionString;
        static RedisCacheConnectionManager()
            => redisConnectionString = AppSettingsManager.GetAppSettingValue("RedisConnectionString");

        /// <summary>
        /// The configuration options
        /// </summary>
        private static Lazy<ConfigurationOptions> _configOptions = new Lazy<ConfigurationOptions>(() =>
        {
            return !string.IsNullOrWhiteSpace(redisConnectionString)
                ? ConfigurationOptions.Parse(redisConnectionString)
                : null;
        });
        /// <summary>
        /// The multiplexer
        /// </summary>
        private static Lazy<IConnectionMultiplexer> _multiplexer = new Lazy<IConnectionMultiplexer>(() =>
        {
            try { return ConnectionMultiplexer.Connect(ConfigOptions); }
            catch { return null; }
        });

        /// <summary>
        /// Gets the configuration options.
        /// </summary>
        /// <value>
        /// The configuration options.
        /// </value>
        public static ConfigurationOptions ConfigOptions { get { return _configOptions.Value; } }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <value>
        /// The connection.
        /// </value>
        public static IConnectionMultiplexer Connection { get { return _multiplexer.Value; } }
    }
}
