using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase db;
        private string listKey = "sortedsetnames";
        public SortedSetTypeController(RedisService redis)
        {
            _redis = redis;
            db = redis.GetDB(3);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();

            if(db.KeyExists(listKey))
            {
                db.SortedSetScan(listKey).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
            }
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Add(string name,int score) 
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            await db.SortedSetAddAsync(listKey,name,score);
            return RedirectToAction("Index");   
        }
        public IActionResult DeleteItem(string name)
        {
            db.SortedSetRemove(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
