using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashTypeController : BaseController
    {
        public string hashKey { get; set; } = "sozluk";
        public HashTypeController(RedisService redisService) : base(redisService)
        { }

        [HttpPost("[action]")]
        public IActionResult AddData(string name, string value)
        {
            db.HashSetAsync(hashKey, name, value);
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult ListData()
        {
            Dictionary<string, string> nameList = new Dictionary<string, string>();

            // KeyExists bir sözlükte veya bir dizide bulunup bulunmadığını kontrol etmek için kullanılır.
            if (db.KeyExists(hashKey))
            {
                db.HashGetAllAsync(hashKey).Result.ToList().ForEach(x =>
                {
                    nameList.Add(x.Name, x.Value);
                });
            }

            return Ok(nameList);
        }

        [HttpGet("[action]")]
        public IActionResult DeleteData(string name)
        {
            var existingValue = db.HashGet(hashKey, name);

            if (existingValue.IsNull)
            {
                return NotFound("Böyle bir veri yok.");
            }

            db.HashDelete(hashKey, name);
            return Ok("Veri başarıyla silindi.");
        }
    }
}
