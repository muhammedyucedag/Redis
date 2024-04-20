using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListTypeController : BaseController
    {
        public string key { get; set; } = "names";

        public ListTypeController(RedisService redisService) : base(redisService)
        {}

        [HttpPost("[action]")]
        public IActionResult AddData(string name)
        {
            db.ListRightPush(key, name);
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult ListData()
        {
            List<string> nameList = new List<string>();

            // KeyExists bir sözlükte veya bir dizide bulunup bulunmadığını kontrol etmek için kullanılır.
            if (db.KeyExists(key))
            {
                nameList = db.ListRange(key).Select(x => x.ToString()).ToList();
            }

            return Ok(nameList);
        }

        [HttpGet("[action]")]
        public IActionResult DeleteData(string name)
        {
            // İsim listede varsa, listeden sil
            var existingData = db.ListRangeAsync(key).Result;
            if (existingData.Contains(name))
            {
                db.ListRemoveAsync(key, name).Wait();
                return Ok("Veri başarılı bir şekilde silindi.");
            }
            else
            {
                return NotFound("Veri bulunamadı."); // Veri bulunamadı durumunda 404 döndür
            }
        }
    }
}
