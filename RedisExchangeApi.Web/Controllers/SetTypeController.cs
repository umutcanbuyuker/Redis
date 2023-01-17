using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase db;
        private string listKey = "names";

        public SetTypeController(RedisService redis)
        {
            _redis = redis;
            db = redis.GetDB(2);
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }
            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {   
            //sliding:
            //if(!db.KeyExists(listKey))
            //{
            //    db.KeyExpire(listKey, DateTime.Now.AddMinutes((5)));
            //}
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            db.SetAdd(listKey, name);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult>  DeleteItem(string name)
        {
            await db.SetRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
    }
}
