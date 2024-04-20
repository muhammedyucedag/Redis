using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        private string listKey = "names";
        
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetRedisDb(0);

        }

        [HttpPost("[action]")]
        public IActionResult AddData(string name)
        {
            db.ListRightPush(listKey, name);
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult ListData()
        {
            List<string> nameList = new List<string>();

            // KeyExists bir sözlükte veya bir dizide bulunup bulunmadığını kontrol etmek için kullanılır.
            if (db.KeyExists(listKey))
            {
                nameList = db.ListRange(listKey).Select(x => x.ToString()).ToList();
            }

            return Ok(nameList);
        }

        [HttpGet("[action]")]
        public IActionResult DeleteData(string name)
        {
            // İsim listede varsa, listeden sil
            var existingData = db.ListRangeAsync(listKey).Result;
            if (existingData.Contains(name))
            {
                db.ListRemoveAsync(listKey, name).Wait();
                return Ok("Veri başarılı bir şekilde silindi.");
            }
            else
            {
                return NotFound("Veri bulunamadı."); // Veri bulunamadı durumunda 404 döndür
            }
        }
    }
}
