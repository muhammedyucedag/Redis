using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;

namespace RedisExchangeAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StringTypeController : BaseController
    {
        public StringTypeController(RedisService redisService) : base(redisService)
        {}

        [HttpGet("[action]")]
        public IActionResult Set()
        {
            db.StringSet("name", "Muhammed Yücedağ");
            db.StringSet("ziyaretci", 100);

            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult Get()
        {         
            var stringGet =  db.StringGet("name");
            var stringGetRange =  db.StringGetRange("name", 0,3);
            var stringLength =  db.StringLength("name");

            //Get endpointi çalıştığı an ziyaretci StringIncrementAsync metodu 1 artar
            //db.StringIncrementAsync("ziyaretci", 1);

            //Get endpointi çalıştığı an ziyaretci StringDecrementAsync metodu ile 1 azalır
            //async metodan bir veri dönüşü bekliyorsan .result gerekli
            //var count = db.StringDecrementAsync("ziyaretci", 1).Result;

            //async metodan bir veri dönüşü beklenmiyorsa .wait gerekli
            db.StringDecrementAsync("ziyaretci", 1).Wait();


            //if (stringGetRange.HasValue)
            //    return Ok(stringGetRange.ToString());
            
                return Ok(stringLength.ToString());         
        }
    }
}
