using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

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

        //Öneri! Byte'a dönüştürmek meşakatli bir iştir öneri şudur direk jsona çevirip get set ile okuyup kaydetmeniz olacaktır. 

        [HttpGet("[action]")]
        public async Task<IActionResult> Set()
        {
            // Cache için seçeneklerin oluşturulması
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddMinutes(1) // Mutlaka UTC zaman kullanılması önerilir
            };

            // Cache'e veri yazılması
            //_distributedCache.SetString("name", "Muhammed", cacheOptions);
            //await _distributedCache.SetStringAsync("name", "Yücedağ", cacheOptions);

            Product product = new Product { Id = Guid.NewGuid(), Name = "Kalem", Price = 200 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            //Json olan veriyi byte dönüştürdük
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set($"Product:{product.Id}", byteProduct, cacheOptions);
        
            //await _distributedCache.SetStringAsync($"Product:{product.Id}", jsonProduct, cacheOptions);

            // HTTP 200 (OK) yanıtı
            return Ok("Data cached successfully."); // İsteğe göre mesajı değiştirebilirsiniz
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get(Guid productId)
        {
            //string name = _distributedCache.GetString("name");
            //string nameAsync = await _distributedCache.GetStringAsync("name");

            Byte[] cachedProductBytes = await _distributedCache.GetAsync($"Product:{productId}");

            // Cache'ten alınan verinin boş olup olmadığını kontrol etme
            if (cachedProductBytes == null)
                return NotFound("Product not found in cache.");

            string cachedProduct = Encoding.UTF8.GetString(cachedProductBytes);

            //string cachedProduct = await _distributedCache.GetStringAsync($"Product:{productId}");   

            // JSON formatındaki verinin yeniden ürün nesnesine dönüştürülmesi
            Product product = JsonConvert.DeserializeObject<Product>(cachedProduct);

            // Alınan ürün nesnesini döndürme
            return Ok(product);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Remove()
        {
            _distributedCache.Remove("name");
            await _distributedCache.RemoveAsync("name");

            return Ok("Data cached removed");
        }

        [HttpGet("[action]")]
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pexels-kaique-rocha-21525766.jpg");

            //ReadAllBytes metodu, belirtilen dosyanın tamamını bir byte dizisine okur ve bu byte dizisini döndürür. 
            byte[] cachedImageBytes = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("resim", cachedImageBytes);
            return Ok();
        }

        [HttpGet("[action]")]
        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("resim");

            return File(imageByte, "image/jpg");
        }
    }
}
