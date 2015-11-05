using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using NinjaLab.Redis.Models;
using StackExchange.Redis;

namespace NinjaLab.Redis.Helpers
{
    public class RedisHelper
    {
        private static ConnectionMultiplexer redis;

        public static IDatabase GetDatabase()
        {
            if (redis == null)
            {
                var server =
                 "ninjalabredis.redis.cache.windows.net,abortConnect=false,ssl=true,password=hUIb6nVH9fqb3/xjgWNsLYZdSi9lhIPL4HxCIJIlD3E=";
                redis = ConnectionMultiplexer.Connect(server);
            }
            return redis.GetDatabase();
        }
        public static Event GetEvent(Guid guid)
        {
            var db = GetDatabase();
            var eventResult = db.StringGet(guid.ToString());
            if (eventResult.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Event>(eventResult);
        }

        public static void SetEvent(Event @event)
        {
            var db = RedisHelper.GetDatabase();
            db.StringSet(@event.Id.ToString(), JsonConvert.SerializeObject(@event));
        }

        public static void DeleteEvent(Guid guid)
        {
            var db = RedisHelper.GetDatabase();
            db.KeyDelete(guid.ToString());
        }
    }
}