using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SortedSetTypeController : BaseController
    {
        public string key { get; set; } = "sortedsetnames";

        public SortedSetTypeController(RedisService redisService) : base(redisService)
        { }

        [HttpGet("[action]")]
        public IActionResult ListData()
        {
            HashSet<string> nameList = new HashSet<string>();

            if (db.KeyExists(key))
            {
                //SortedSetScan metodu, Redis'teki sıralı kümenin tüm elemanlarını parçalar halinde almak ve işlemek için kullanılan bir iterasyon aracıdır.
                //db.SortedSetScan(key).ToList().ForEach(member =>
                //{
                //    nameList.Add(member.ToString());
                //});

                db.SortedSetRangeByRank(key, order: Order.Descending).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });
            }

            return Ok(nameList);
        }

        [HttpPost("[action]")]
        public IActionResult AddData(string name, int score)
        {
            db.SortedSetAdd(key, name, score);

            db.KeyExpire(key, DateTime.Now.AddMinutes(1));

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DeleteDataAsync(string name)
        {

            await db.SortedSetRemoveAsync(key, name);
            return Ok();
        }
    }
}