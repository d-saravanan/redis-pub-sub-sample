using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisLibrary
{
    /// <summary>
    /// The Redis Cache Manager
    /// </summary>
    public static class RedisCacheManager
    {
        /// <summary>
        /// The redis cache
        /// </summary>
        private static readonly IConnectionMultiplexer redisCache;
        /// <summary>
        /// The redis database
        /// </summary>
        private static readonly IDatabase _redisDatabase;
        /// <summary>
        /// The callback channel
        /// </summary>
        private static readonly string CallbackChannel = "Hash:Cleared:Callback";

        /// <summary>
        /// Initializes the <see cref="RedisCacheManager"/> class.
        /// </summary>
        static RedisCacheManager()
        {
            redisCache = RedisCacheConnectionManager.Connection;
            _redisDatabase = redisCache.GetDatabase();
            InitializeSubscriptionAsync().Wait();
        }

        /// <summary>
        /// Gets or sets the subscription callback handler.
        /// </summary>
        /// <value>
        /// The subscription callback handler.
        /// </value>
        public static Action<string, string> SubscriptionCallbackHandler { get; set; }

        /// <summary>
        /// Initializes the subscription asynchronously.
        /// </summary>
        /// <returns></returns>
        private static async Task InitializeSubscriptionAsync()
        {
            var redisSubscription = redisCache.GetSubscriber();
            try
            {
                await redisSubscription.SubscribeAsync((CallbackChannel + "*"), (channel, payload) =>
                {
                    SubscriptionCallbackHandler(channel, payload);
                });
            }
            catch (TimeoutException e)
            {
                // try again
                InitializeSubscriptionAsync().Wait();
            }
        }

        /// <summary>
        /// Adds the asynchronously
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashKey">The hash key.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static async Task AddAsync<T>(string hashKey, string key, T value)
            => await _redisDatabase.HashSetAsync(hashKey, new HashEntry[] { new HashEntry(key, Newtonsoft.Json.JsonConvert.SerializeObject(value)) });

        /// <summary>
        /// Gets data from the cache asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hashKey">The hash key.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string hashKey, string key)
        {
            var data = await _redisDatabase.HashGetAsync(hashKey, key);
            if (data.IsNull || data.IsNullOrEmpty) return default(T);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        /// Clears the hash asynchronously.
        /// </summary>
        /// <param name="hashKey">The hash key.</param>
        /// <returns></returns>
        public static async Task ClearHashAsync(string hashKey)
        {
            await _redisDatabase.KeyDeleteAsync(hashKey);
            await _redisDatabase.PublishAsync(CallbackChannel, $"{hashKey}");
        }

        /// <summary>
        /// Clears the key in hash asynchronously.
        /// </summary>
        /// <param name="hashKey">The hash key.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static async Task ClearKeyInHashAsync(string hashKey, string key)
        {
            await _redisDatabase.HashDeleteAsync(hashKey, key);
            //await _redisDatabase.PublishAsync(CallbackChannel, $"{hashKey}-{key}");
        }
    }
}
