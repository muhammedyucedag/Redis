using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetTypeController : BaseController
    {
        public string key { get; set; } = "setNames";

        public SetTypeController(RedisService redisService) : base(redisService)
        { }

        [HttpGet("[action]")]
        public IActionResult ListData()
        {
            HashSet<string> nameList = new HashSet<string>();

            if (db.KeyExists(key))
            {
                db.SetMembers(key).ToList().ForEach(member =>
                {
                    nameList.Add(member.ToString());
                });
            }

            return Ok(nameList);
        }

        [HttpPost("[action]")]
        public IActionResult AddData(string name)
        {
            if (!db.KeyExists(key))
                db.KeyExpire(key, DateTime.Now.AddMinutes(1));

            db.SetAdd(key, name);

            return Ok();
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> DeleteDataAsync(string name)
        {

            await db.SetRemoveAsync(key, name);
            return Ok();
        }
    }
}
