using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Poseidon.infra.redis
{
    class redis
    {
        // redis config
        private static ConfigurationOptions connDCS = ConfigurationOptions.Parse(Class1.Ip + ":6379,password=,connectTimeout=2000");
        //the lock for singleton
        private static readonly object Locker = new object();
        //singleton
        private static ConnectionMultiplexer redisConn;
        //singleton
        public static ConnectionMultiplexer GetRedisConn()
        {
            if (redisConn == null)
            {
                lock (Locker)
                {
                    if (redisConn == null || !redisConn.IsConnected)
                    {
                        redisConn = ConnectionMultiplexer.Connect(connDCS);
                    }
                }
            }
            return redisConn;
        }
        public static void Subscribe(RedisChannel channel, Action<RedisChannel, RedisValue> handler)
        {
            ConnectionMultiplexer redisCli = GetRedisConn();
            var sub = redisCli.GetSubscriber();
            sub.Subscribe(channel, handler);
        }
        public static void UnSubscribe(RedisChannel channel)
        {
            ConnectionMultiplexer redisCli = GetRedisConn();
            var sub = redisCli.GetSubscriber();
            sub.Unsubscribe(channel);
        }
    }
}
