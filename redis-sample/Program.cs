using RedisLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp_Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var hashKey = "user:tvr";

            var rcs = new RedisCacheSampler();
            rcs.AddToCache(hashKey, "key1", "key1");
            rcs.AddToCache(hashKey, "key2", "key2");
            rcs.AddToCache(hashKey, "key3", "key3");
            rcs.AddToCache(hashKey, "key4", "key4");
            rcs.AddToCache(hashKey, "key5", "key5");

            Console.WriteLine("The value for key1 is : {0}", rcs.GetFromCache(hashKey, "key1"));
            Console.WriteLine("The value for key3 is : {0}", rcs.GetFromCache(hashKey, "key3"));
            Console.WriteLine("The value for key5 is : {0}", rcs.GetFromCache(hashKey, "key5"));

            Console.WriteLine("The key1 is being deleted now");
            rcs.ClearKeyInHash(hashKey, "key1");

            Console.WriteLine("The value for key1 is : {0}", rcs.GetFromCache(hashKey, "key1"));

            Console.WriteLine("The hash is being deleted now");
            rcs.ClearHash(hashKey);

            Console.WriteLine(hashKey);
            Console.ReadLine();

        }
    }

    public class RedisCacheSampler
    {
        public void AddToCache(string hashKey, string key, string value)
            => RedisCacheManager.AddAsync(hashKey, key, value).Wait();

        public string GetFromCache(string hashKey, string key)
            => (RedisCacheManager.GetAsync<string>(hashKey, key).Result);

        public void ClearHash(string hashKey)
            => RedisCacheManager.ClearHashAsync(hashKey).Wait();

        public void ClearKeyInHash(string hashKey, string key)
            => RedisCacheManager.ClearKeyInHashAsync(hashKey, key).Wait();
    }
}
