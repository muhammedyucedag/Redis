using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Zaman verisini önbelleğe ayarlar.
        /// </summary>
        [ActionName(nameof(Set))]
        [HttpPost]
        public IActionResult Set()
        {
            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();

            // Cache ömrünü belirliyoruz.
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            // 10 saniye içinde veri çekilmezse silinir çekilirse 10 saniye eklenir.
            cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(10);

            _memoryCache.Set<string>("Zaman", DateTime.Now.ToString(), cacheOptions);

            return Ok();
        }

        /// <summary>
        /// Zaman verisini önbellekten getirir.
        /// </summary>
        [ActionName(nameof(Get))]
        [HttpGet]
        public IActionResult Get()
        {
            if (_memoryCache.TryGetValue("Zaman", out string zamanCache))
            {
                return Ok(zamanCache);
            }
            else
            {
                return NotFound("Zaman verisi bulunamadı.");
            }
        }

    }
}
