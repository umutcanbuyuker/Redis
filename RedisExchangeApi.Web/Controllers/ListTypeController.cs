using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase db;
        private string listKey = "names";
        public ListTypeController(RedisService redis)
        {
            _redis = redis;
            db = redis.GetDB(1);
        }
        public IActionResult Index()
        {
            List<string> namesList = new List<string>();
            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x=>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            db.ListRightPush(listKey, name);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteItem(string name)
        {
            db.ListRemoveAsync(listKey, name).Wait();
            return RedirectToAction("Index");
        }
        public IActionResult DeleteFirstItem()
        {
            //db.ListLeftPop(listKey);
            db.ListRightPop(listKey);
            return RedirectToAction("Index");
        }
    }
}
