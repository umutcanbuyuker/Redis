using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisService _redis;
        protected readonly IDatabase db;

        public BaseController(RedisService redis)
        {
            _redis = redis;
            db = redis.GetDB(2);
        }
    }
}
