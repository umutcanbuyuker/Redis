using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase db;
        public StringTypeController(RedisService redis)
        {
            _redis = redis;
            db = redis.GetDB(0);
        }
        public IActionResult Index()
        {
            //var db = _redis.GetDB(0);
            //var db = _redisService.GetDB(0);
            db.StringSet("nameapi", "UmutCan");
            db.StringSet("ziyaretciapi",100);

            return View();
        }

        public IActionResult Show()
        {
            
            
            var value = db.StringLength("nameapi");

            //var value = db.StringGetRange("nameapi",0,3);
            //db.StringIncrement("ziyaretciapi", 1);
            var count = db.StringDecrementAsync("ziyaretciapi", 1).Result; //metottan bir değer dönüyorsa async ** .result ile bir değişkene atabiliriz.
            ////db.StringDecrementAsync("ziyaretciapi", 10).Wait();
            //if(value.HasValue)
            {
                ViewBag.value = value.ToString();
            }
            return View();
        }
    }
}
