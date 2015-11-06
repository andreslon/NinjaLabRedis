using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using NinjaLab.Redis.Models;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace NinjaLab.Redis.Helpers
{
    public class RedisHelper
    {
        private const string RedisUri = "BoCg/iKfaH4dK96UZoBbdkSZx7MFTiy4Nn56h3AR0C8=@ninjalabredis.redis.cache.windows.net?ssl=true";
        private static PooledRedisClientManager clientsManager;
        private static IRedisClient redisClient;
        public static void RedisConnection()
        {
            if (redisClient == null)
            {
                clientsManager = new PooledRedisClientManager(RedisUri);
                if (redisClient == null)
                {
                    redisClient = clientsManager.GetClient();
                }
            }
        }


        public static void test()
        {

            var id = Guid.NewGuid();
            Set(new Event() { Id = id, Comment = "SSD", Title = "dasdsadsa" });
            Set(new Event() { Id = id, Comment = "gggggg", Title = "aaaaaaa" });
            var dd = Get(id.ToString());
            Remove(id.ToString());
        }

        public static List<Event> GetAll()
        {
            RedisConnection();

            var lstEvents = new List<Event>();
            foreach (var key in redisClient.GetAllKeys())
            {
                try
                {
                    lstEvents.Add(Get(key));
                }
                catch (Exception)
                {
                     
                }
              
            }
            return lstEvents;
        }

        public static Event Get(string id)
        {
            RedisConnection();

            var response = redisClient.Get<string>(id);

          
            return JsonConvert.DeserializeObject<Event>(response);
            //return JsonConvert.DeserializeObject<Event>(redisClient.Get<string>(id.ToString()));
            //return redisClient.Get<Event>(id.ToString());
        }
        public static bool Remove(string id)
        {
            RedisConnection();

            return redisClient.Remove(id);
        }

        public static bool Set(Event @event)
        {
            RedisConnection();
            // Save a guid instance in Redis  
           return redisClient.Set(@event.Id.ToString(), JsonConvert.SerializeObject(@event));
            //redisClient.Store(@event);
        }






    }
}