using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Set()
        {
            // Cache için seçeneklerin oluşturulması
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(1) // Mutlaka UTC zaman kullanılması önerilir
            };

            // Cache'e veri yazılması
            _distributedCache.SetString("name", "Muhammed", cacheOptions);
            await _distributedCache.SetStringAsync("name", "Yücedağ", cacheOptions);

            // HTTP 200 (OK) yanıtı
            return Ok("Data cached successfully."); // İsteğe göre mesajı değiştirebilirsiniz
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            string name = _distributedCache.GetString("name");
            string nameAsync = await _distributedCache.GetStringAsync("name");
            return Ok(nameAsync);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Remove()
        {
            _distributedCache.Remove("name");
            await _distributedCache.RemoveAsync("name");

            return Ok("Data cached removed");
        }
    }
}
