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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult Set()
        {
            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();

            // Cache ömrünü belirliyoruz.
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

            // 10 saniye içinde veri çekilmezse silinir çekilirse 10 saniye eklenir.
            // cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(10);

            // Redis'te, önbellek önceliği (CacheItemPriority) yerine anahtarlar için zamanlanmış son kullanma süreleri belirlenir.
            cacheOptions.Priority = CacheItemPriority.High;

            // RegisterPostEvictionCallback metodu, bir bellek önbelleği öğesi önbellekten kaldırıldığında belirli bir geri çağrıyı tetiklemek için kullanılır.
            cacheOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set("callback", $"{key}->{value} => sebep:{reason}");
            });

            _memoryCache.Set<string>("Zaman", DateTime.Now.ToString(), cacheOptions);

            return Ok();
        }

        /// /// <summary>
        /// Zaman verisini önbellekten getirir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult Get()
        {
            _memoryCache.TryGetValue("Zaman", out string zamanCache);

            _memoryCache.TryGetValue("callback", out string callback);

            return Ok(new { Zaman = zamanCache, Callback = callback });
        }

    }
}
