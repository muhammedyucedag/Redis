using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        private string listKey = "hashnames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetRedisDb(0);

        }

        [HttpGet("[action]")]
        public IActionResult ListData()
        {
            HashSet<string> nameList = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(member =>
                {
                    nameList.Add(member.ToString());
                });
            }

            return Ok(nameList);
        }

        [HttpPost("[action]")]
        public IActionResult AddData(string name)
        {
            if (!db.KeyExists(listKey))
                db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));

            db.SetAdd(listKey, name);

            return Ok();
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> DeleteDataAsync(string name)
        {

            await db.SetRemoveAsync(listKey, name);
            return Ok();
        }
    }
}
