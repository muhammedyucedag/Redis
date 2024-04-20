using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SortedSetTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        private string listKey = "sortedsetnames";

        public SortedSetTypeController(RedisService redisService)
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
                //SortedSetScan metodu, Redis'teki sıralı kümenin tüm elemanlarını parçalar halinde almak ve işlemek için kullanılan bir iterasyon aracıdır.
                //db.SortedSetScan(listKey).ToList().ForEach(member =>
                //{
                //    nameList.Add(member.ToString());
                //});

                db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });
            }

            return Ok(nameList);
        }

        [HttpPost("[action]")]
        public IActionResult AddData(string name, int score)
        {
            db.SortedSetAdd(listKey, name, score);

            db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DeleteDataAsync(string name)
        {

            await db.SortedSetRemoveAsync(listKey, name);
            return Ok();
        }
    }
}