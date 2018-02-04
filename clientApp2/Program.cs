using RedisLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp_Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisCacheManager.SubscriptionCallbackHandler = Subscriber;
            Console.ReadLine();
        }

        /// <summary>
        /// Subscribers to the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="payload">The payload.</param>
        static void Subscriber(string channel, string payload)
            => Console.WriteLine("Some App has cleared the tvr cache. The channel is: {0} has the payload is: {1}", channel, payload);
    }
}
