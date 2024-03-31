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
            // Cache Kontrol 1. Yol
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("Zaman")))
            //{
            //    _memoryCache.Set<string>("Zaman", DateTime.Now.ToString());
            //}

            // 2. yol
            if (!_memoryCache.TryGetValue("Zaman", out string zamanCache))
            {
                _memoryCache.Set<string>("Zaman", DateTime.Now.ToString());
            }

            try
            {
                _memoryCache.Set<string>("Zaman", DateTime.Now.ToString());

                return Ok();
            }
            catch (Exception ex)
            {
                // Cache'e yazma işlemi başarısız olduysa uygun bir hata mesajı döndürün.
                return StatusCode(500, "Zaman verisi cache'e yazılamadı. Hata: " + ex.Message);
            }
        }

        /// <summary>
        /// Zaman verisini önbellekten getirir.
        /// </summary>
        [ActionName(nameof(Get))]
        [HttpGet]
        public IActionResult Get()
        {
            // cache almaya çalışır yoksa oluşturur (GetOrCreate)
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                return DateTime.Now.ToString();
            });
            _memoryCache.Remove("Zaman");

            var zaman = _memoryCache.Get<string>("Zaman");

            if (zaman == null)
            {
                return NotFound("Zaman verisi bulunamadı.");
            }

            return Ok(zaman);
        }


    }
}
