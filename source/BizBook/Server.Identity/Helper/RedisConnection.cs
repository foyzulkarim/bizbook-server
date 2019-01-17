namespace Server.Identity.Hubs
{
    using System;
    using System.Configuration;

    using Newtonsoft.Json;

    using StackExchange.Redis;

    public class RedisConnection
    {
        static RedisConnection()
        {
            string appSetting = ConfigurationManager.AppSettings["RedisConnection"];
            RedisConnection.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    return ConnectionMultiplexer.Connect(appSetting);
                });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }

    public static class RedisService
    {
        static RedisConnection obj;
        static IDatabase db;

        static RedisService()
        {
            obj = new RedisConnection();
            db = obj.Connection.GetDatabase();
        }

        public static void UpdateRedis(string keySuffix, object data)
        {
            string s = JsonConvert.SerializeObject(data);
            string prefix = ConfigurationManager.AppSettings["RedisPrefix"];
            bool stringSet = db.StringSet(prefix + keySuffix, s);
        }

        public static string GetValue(string key)
        {
            string prefix = ConfigurationManager.AppSettings["RedisPrefix"];
            string value = db.StringGet(prefix + key);
            return value;
        }
    }
}